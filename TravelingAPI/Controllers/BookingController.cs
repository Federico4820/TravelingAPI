using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Interfaces;

namespace TravelingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, IMapper mapper, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetBookings()
        {
            _logger.LogInformation("Recupero tutte le prenotazioni dal sistema.");
            var bookings = await _bookingService.GetAllBookingsAsync();
            if (bookings == null || !bookings.Any())
            {
                _logger.LogWarning("Nessuna prenotazione trovata.");
                return NotFound();
            }

            _logger.LogInformation("{Count} prenotazioni recuperate con successo.", bookings.Count);
            return Ok(bookings);
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<BookingDto>>> GetBookingsByUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("Claim dell'utente mancante.");
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            _logger.LogInformation("Recupero le prenotazioni per l'utente con ID {UserId}.", userId);
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            if (bookings == null || !bookings.Any())
            {
                _logger.LogWarning("Nessuna prenotazione trovata per l'utente con ID {UserId}.", userId);
                return NotFound();
            }

            _logger.LogInformation("{Count} prenotazioni trovate per l'utente con ID {UserId}.", bookings.Count, userId);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            _logger.LogInformation("Recupero prenotazione con ID {Id}.", id);
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning("Prenotazione con ID {Id} non trovata.", id);
                return NotFound();
            }

            _logger.LogInformation("Prenotazione con ID {Id} recuperata correttamente.", id);
            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> CreateBooking(BookingCreateDto bookingCreateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("Claim dell'utente mancante durante la creazione della prenotazione.");
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            _logger.LogInformation("Creazione di una nuova prenotazione.");
            var createdBooking = await _bookingService.CreateBookingAsync(bookingCreateDto, userId);
            if (createdBooking == null)
            {
                _logger.LogError("Errore durante la creazione della prenotazione.");
                return BadRequest("Impossibile creare la prenotazione.");
            }

            _logger.LogInformation("Prenotazione con ID {Id} creata con successo per l'utente {UserId}.", createdBooking.Id, userId);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(Guid id, BookingUpdateDto bookingUpdateDto)
        {
            _logger.LogInformation("Aggiornamento della prenotazione con ID {Id}.", id);
            var updatedBooking = await _bookingService.UpdateBookingAsync(bookingUpdateDto);
            if (updatedBooking == null)
            {
                _logger.LogWarning("Prenotazione con ID {Id} non trovata per l'aggiornamento.", id);
                return NotFound();
            }

            _logger.LogInformation("Prenotazione con ID {Id} aggiornata correttamente.", id);
            return Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("Claim dell'utente mancante durante l'eliminazione della prenotazione.");
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var isAdmin = User.IsInRole("Admin");

            _logger.LogInformation("Richiesta di cancellazione della prenotazione ID {Id} da parte di {UserId} (Admin: {IsAdmin}).", id, userId, isAdmin);

            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning("Prenotazione con ID {Id} non trovata.", id);
                return NotFound();
            }

            if (!isAdmin && booking.UserId != userId)
            {
                _logger.LogWarning("Accesso non autorizzato: l'utente {UserId} ha tentato di eliminare la prenotazione di un altro utente.", userId);
                return Forbid();
            }

            var result = await _bookingService.DeleteBookingAsync(id);
            if (!result)
            {
                _logger.LogError("Errore imprevisto durante l'eliminazione della prenotazione con ID {Id}.", id);
                return StatusCode(500, "Errore durante l'eliminazione.");
            }

            _logger.LogInformation("Prenotazione con ID {Id} cancellata con successo da {UserId}.", id, userId);
            return NoContent();
        }

    }
}
