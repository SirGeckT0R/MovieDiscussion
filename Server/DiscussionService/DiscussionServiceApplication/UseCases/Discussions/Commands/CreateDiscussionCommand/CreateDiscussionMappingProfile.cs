using AutoMapper;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public class CreateDiscussionMappingProfile : Profile
    {
        public CreateDiscussionMappingProfile() 
        {
            CreateMap<CreateDiscussionCommand, Discussion>();
        }
    }
}
