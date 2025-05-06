using System.ComponentModel.DataAnnotations;

namespace TravelingAPI.DTOs.Travel
{
    public class TripUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Price { get; set; }

        public IFormFile? Image { get; set; }
    }
}
