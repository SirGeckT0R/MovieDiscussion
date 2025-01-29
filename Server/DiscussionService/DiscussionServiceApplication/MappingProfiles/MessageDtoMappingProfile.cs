using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.MappingProfiles
{
    public class MessageDtoMappingProfile : Profile
    {
        public MessageDtoMappingProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}
