using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class UserProfileDtoMappingProfile : Profile
    {
        public UserProfileDtoMappingProfile() 
        {
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
        }
    }
}
