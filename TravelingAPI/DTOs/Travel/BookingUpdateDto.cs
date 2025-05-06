using System.ComponentModel.DataAnnotations;

namespace TravelingAPI.DTOs.Travel
{
    public class BookingUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        public int NumberOfPeople { get; set; }

        public DateTime BookingDate { get; set; }

        public Guid TripId { get; set; }
    }
}
