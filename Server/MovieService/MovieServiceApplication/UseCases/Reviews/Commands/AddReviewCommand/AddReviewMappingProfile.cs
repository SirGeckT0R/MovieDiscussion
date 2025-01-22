using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand
{
    public class AddReviewMappingProfile : Profile
    {
        public AddReviewMappingProfile() 
        {
            CreateMap<AddReviewCommand, Review>();
        }
    }
}
