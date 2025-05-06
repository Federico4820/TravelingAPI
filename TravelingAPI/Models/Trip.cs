using System.ComponentModel.DataAnnotations;

namespace TravelingAPI.Models
{
    public class Trip
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
