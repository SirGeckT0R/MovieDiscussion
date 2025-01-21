using AutoMapper;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.MappingProfiles
{
    public class UserClaimsDtoMappingProfile : Profile
    {
        public UserClaimsDtoMappingProfile()
        {
            CreateMap<User, UserClaimsDto>();
        }
    }
}
