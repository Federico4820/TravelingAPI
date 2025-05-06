namespace TravelingAPI.DTOs.Travel
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }
        public string TripDestination { get; set; }
        public decimal TripPrice { get; set; }

        public Guid UserId { get; set; }
        public string UserFullName { get; set; }

        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }

        public decimal TotalPrice => TripPrice * NumberOfPeople;
    }
}
