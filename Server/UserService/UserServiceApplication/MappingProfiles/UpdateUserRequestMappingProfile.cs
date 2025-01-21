using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.MappingProfiles
{
    public class UpdateUserRequestMappingProfile : Profile
    {
        public UpdateUserRequestMappingProfile()
        {
            CreateMap<UpdateUserRequest, User>();
        }
    }
}
