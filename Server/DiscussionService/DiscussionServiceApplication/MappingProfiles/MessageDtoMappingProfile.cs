using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.MappingProfiles
{
    public class MessageDtoMappingProfile : Profile
    {
        public MessageDtoMappingProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(source => source.SentBy))
                .ReverseMap();
        }
    }
}
