using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.MappingProfiles
{
    public class RegisterRequestMappingProfile : Profile
    {
        public RegisterRequestMappingProfile() {
            CreateMap<RegisterRequest, User>()
                .AfterMap((com, user) => user.Role = ERole.User);
        }
    }
}
