using AutoMapper;
using DiscussionServiceDomain.Models;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand
{
    public class UpdateDiscussionMappingProfile : Profile
    {
        public UpdateDiscussionMappingProfile()
        {
            CreateMap<UpdateDiscussionCommand, Discussion>();
        }
    }
}
