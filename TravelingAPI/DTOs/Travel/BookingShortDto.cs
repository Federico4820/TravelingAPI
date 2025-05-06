namespace TravelingAPI.DTOs.Travel
{
    public class BookingShortDto
    {
        public Guid Id { get; set; }
        public string TripDestination { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
