using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewMappingProfile : Profile
    {
        public UpdateReviewMappingProfile() 
        {
            CreateMap<UpdateReviewCommand, Review>();
        }
    }
}
