using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class ReviewDtoMappingProfile :Profile
    {
        public ReviewDtoMappingProfile() 
        {
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}
