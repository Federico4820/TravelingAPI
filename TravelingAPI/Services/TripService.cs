using Microsoft.Extensions.Logging;
using TravelingAPI.Data;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Interfaces;
using TravelingAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TravelingAPI.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TripService> _logger;
        private readonly string _uploadFolder;
        private readonly IWebHostEnvironment _environment;

        public TripService(ApplicationDbContext context, IMapper mapper, ILogger<TripService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        public async Task<List<TripDto>> GetAllTripsAsync()
        {
            var trips = await _context.Trips.ToListAsync();
            _logger.LogInformation("Trovati {Count} viaggi nel sistema", trips.Count);
            return _mapper.Map<List<TripDto>>(trips);
        }

        public async Task<TripDto?> GetTripByIdAsync(Guid id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                _logger.LogWarning("Viaggio con ID {Id} non trovato", id);
                return null;
            }

            _logger.LogInformation("Recuperato viaggio {Id}", id);
            return _mapper.Map<TripDto>(trip);
        }

        public async Task<TripDto> CreateTripAsync(TripCreateDto dto)
        {
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.Image.FileName)}";
                var imagePath = Path.Combine(_uploadFolder, imageName);

                using var stream = new FileStream(imagePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                dto.ImagePath = $"/uploads/{imageName}";
            }
            else
            {
                dto.ImagePath = "/uploads/default_image.jpg";
            }

            var trip = _mapper.Map<Trip>(dto);
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Creato nuovo viaggio con ID: {Id}", trip.Id);
            return _mapper.Map<TripDto>(trip);
        }

        public async Task<TripDto?> UpdateTripAsync(TripUpdateDto dto)
        {
            var trip = await _context.Trips.FindAsync(dto.Id);
            if (trip == null)
            {
                _logger.LogWarning("Tentativo di aggiornare viaggio non trovato con ID: {Id}", dto.Id);
                return null;
            }

            if (!decimal.TryParse(dto.Price, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedPrice))
            {
                _logger.LogWarning("Prezzo non valido ricevuto per il viaggio {Id}: '{Price}'", dto.Id, dto.Price);
                return null;
            }

            trip.Destination = dto.Destination;
            trip.Description = dto.Description;
            trip.Price = parsedPrice;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.Image.FileName)}";
                var imagePath = Path.Combine(_uploadFolder, imageName);

                using var stream = new FileStream(imagePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                trip.ImagePath = $"/uploads/{imageName}";
            }

            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Viaggio aggiornato con ID: {Id}", trip.Id);
            return _mapper.Map<TripDto>(trip);
        }

        public async Task<bool> DeleteTripAsync(Guid id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                _logger.LogWarning("Tentativo di eliminare viaggio inesistente con ID: {Id}", id);
                return false;
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Viaggio eliminato con ID: {Id}", id);
            return true;
        }
    }
}
