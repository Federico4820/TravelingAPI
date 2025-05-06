using AutoMapper;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Models;

namespace TravelingAPI.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.TripDestination, opt => opt.MapFrom(src => src.Trip.Destination))
                .ForMember(dest => dest.TripPrice, opt => opt.MapFrom(src => src.Trip.Price))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src => src.NumberOfPeople))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate));

            CreateMap<BookingCreateDto, Booking>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src => src.NumberOfPeople));

            CreateMap<BookingUpdateDto, Booking>()
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src => src.NumberOfPeople))
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
                .AfterMap((src, dest, context) =>
                {
                    if (context.Items.TryGetValue("TripPrice", out var priceObj) && priceObj is decimal tripPrice)
                    {
                        dest.TotalPrice = tripPrice * src.NumberOfPeople;
                    }
                });

            CreateMap<BookingDeleteDto, Booking>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
