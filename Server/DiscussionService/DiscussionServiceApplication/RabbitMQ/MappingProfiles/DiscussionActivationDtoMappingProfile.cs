using AutoMapper;
using DiscussionServiceApplication.RabbitMQ.Dto;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.RabbitMQ.MappingProfiles
{
    public class DiscussionActivationDtoMappingProfile : Profile
    {
        public DiscussionActivationDtoMappingProfile()
        {
            CreateMap<DiscussionActivationDto, Discussion>().ReverseMap();
        }
    }
}
