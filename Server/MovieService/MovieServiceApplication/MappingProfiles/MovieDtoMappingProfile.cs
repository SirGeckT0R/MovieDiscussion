using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class MovieDtoMappingProfile : Profile
    {
        public MovieDtoMappingProfile() 
        { 
            CreateMap<Movie, MovieDto>().ReverseMap();
        }
    }
}
