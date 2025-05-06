using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TravelingAPI.Models.Auth;

namespace TravelingAPI.Models
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Required]
        public Guid TripId { get; set; }

        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime BookingDate { get; set; }

        public int NumberOfPeople { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
