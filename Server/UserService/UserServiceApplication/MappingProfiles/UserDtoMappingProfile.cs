using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.MappingProfiles
{
    public class UserDtoMappingProfile : Profile
    {
        public UserDtoMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
