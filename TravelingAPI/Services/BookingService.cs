using Microsoft.Extensions.Logging;
using TravelingAPI.Data;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Interfaces;
using TravelingAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace TravelingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ApplicationDbContext context, IMapper mapper, ILogger<BookingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.User)
                .ToListAsync();

            _logger.LogInformation("Recuperate tutte le prenotazioni dal database.");
            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task<List<BookingDto>> GetBookingsByUserIdAsync(string userId)
        {
            var bookings = await _context.Bookings
            .Include(b => b.Trip)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .ToListAsync();

            _logger.LogInformation("Recuperate {Count} prenotazioni per l'utente {UserId}.", bookings.Count, userId);
            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                _logger.LogWarning("Prenotazione con ID {Id} non trovata.", id);
                return null;
            }

            _logger.LogInformation("Prenotazione con ID {Id} recuperata correttamente.", id);
            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<BookingDto> CreateBookingAsync(BookingCreateDto dto, string userId)
        {
            var booking = _mapper.Map<Booking>(dto);
            booking.Id = Guid.NewGuid();
            booking.UserId = userId;
            booking.CreatedAt = DateTime.UtcNow;

            var trip = await _context.Trips.FindAsync(dto.TripId);
            if (trip == null)
            {
                _logger.LogWarning("Viaggio con ID {TripId} non trovato.", dto.TripId);
                return null;
            }

            booking.TotalPrice = trip.Price * dto.NumberOfPeople;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Creata una nuova prenotazione con ID {Id} per l'utente {UserId}.", booking.Id, userId);
            return _mapper.Map<BookingDto>(booking);
        }


        public async Task<BookingDto?> UpdateBookingAsync(BookingUpdateDto dto)
        {
            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == dto.Id);

            if (booking == null)
            {
                _logger.LogWarning("Prenotazione con ID {Id} non trovata.", dto.Id);
                return null;
            }

            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == dto.TripId);
            if (trip == null)
            {
                _logger.LogWarning("Viaggio con ID {TripId} non trovato.", dto.TripId);
                return null;
            }

            _mapper.Map(dto, booking, opt =>
            {
                opt.Items["TripPrice"] = trip.Price;
            });

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Prenotazione con ID {Id} aggiornata correttamente.", booking.Id);
            return _mapper.Map<BookingDto>(booking);
        }


        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
