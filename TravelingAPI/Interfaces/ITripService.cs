using TravelingAPI.DTOs.Travel;

namespace TravelingAPI.Interfaces
{
    public interface ITripService
    {
        Task<List<TripDto>> GetAllTripsAsync();
        Task<TripDto?> GetTripByIdAsync(Guid id);
        Task<TripDto> CreateTripAsync(TripCreateDto dto);
        Task<TripDto?> UpdateTripAsync(TripUpdateDto dto);
        Task<bool> DeleteTripAsync(Guid id);
    }
}
