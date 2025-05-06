namespace TravelingAPI.DTOs.Travel
{
    public class TripCreateDto
    {
        public required string Destination { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
