using AutoMapper;
using JoggingTimeTracker.API.DTOs.JoggingTime;
using JoggingTimeTracker.API.DTOs.User;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;

namespace JoggingTimeTracker.API.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JoggingTimeAddDto, JoggingTime>()
                .ForMember(dest => dest.DistanceInMeter, opt => opt.MapFrom(src => src.DistanceInMeter))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeSpan.Parse(src.Time)));

            CreateMap<JoggingTimeEditDto, JoggingTime>()
                .ForMember(dest => dest.DistanceInMeter, opt => opt.MapFrom(src => src.DistanceInMeter))
                .ForMember(dest => dest.JoggingTimeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeSpan.Parse(src.Time)));

            CreateMap<JoggingTime, JoggingTimeDto>()
                .ForMember(dest => dest.DistanceInMeter, opt => opt.MapFrom(src => src.DistanceInMeter))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.JoggingTimeId))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToString(@"hh\:mm\:ss")));


            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserCreateDto, ApplicationUser > ();
            CreateMap<UserUpdateDto, ApplicationUser > ()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }

}
