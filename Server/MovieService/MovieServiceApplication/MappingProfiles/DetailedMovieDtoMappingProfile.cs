using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class DetailedMovieDtoMappingProfile : Profile
    {
        public DetailedMovieDtoMappingProfile()
        {
            CreateMap<Movie, DetailedMovieDto>()
                     .ForMember(dest => dest.Genres, y => y.MapFrom(src=> src.Genres.Select(x => new GenreDto { Id = x }).ToList()));
        }
    }
}
