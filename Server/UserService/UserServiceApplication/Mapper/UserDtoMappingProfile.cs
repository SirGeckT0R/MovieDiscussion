using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Mapper
{
    public class UserDtoMappingProfile : Profile
    {
        public UserDtoMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
