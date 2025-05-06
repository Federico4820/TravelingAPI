using System.ComponentModel.DataAnnotations;

namespace TravelingAPI.DTOs.Travel
{
    public class BookingCreateDto
    {
        [Required]
        public Guid TripId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int NumberOfPeople { get; set; }
    }
}
