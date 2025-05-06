namespace TravelingAPI.DTOs.Travel
{
    public class TripDto
    {
        public Guid Id { get; set; }
        public string Destination { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
