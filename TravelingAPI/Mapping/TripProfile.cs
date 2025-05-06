using AutoMapper;
using TravelingAPI.DTOs.Travel;
using TravelingAPI.Models;

public class TripProfile : Profile
{
    public TripProfile()
    {
        CreateMap<Trip, TripDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));

        CreateMap<TripCreateDto, Trip>();

        CreateMap<TripUpdateDto, Trip>();
    }
}
