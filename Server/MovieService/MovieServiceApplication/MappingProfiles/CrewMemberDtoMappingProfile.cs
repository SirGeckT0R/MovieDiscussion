using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class CrewMemberDtoMappingProfile : Profile
    {
        public CrewMemberDtoMappingProfile() 
        {
            CreateMap<CrewMember, CrewMemberDto>().ReverseMap();
        }
    }
}
