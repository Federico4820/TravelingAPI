using TravelingAPI.DTOs.Travel;

namespace TravelingAPI.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();
        Task<List<BookingDto>> GetBookingsByUserIdAsync(string userId);
        Task<BookingDto?> GetBookingByIdAsync(Guid id);
        Task<BookingDto> CreateBookingAsync(BookingCreateDto dto, string userId);
        Task<BookingDto?> UpdateBookingAsync(BookingUpdateDto dto);
        Task<bool> DeleteBookingAsync(Guid id);
    }
}
