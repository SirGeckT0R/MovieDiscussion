using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.MappingProfiles
{
    public class DiscussionDtoMappingProfile : Profile
    {
        public DiscussionDtoMappingProfile() 
        {
            CreateMap<Discussion, DiscussionDto>().ReverseMap();
        }
    }
}
