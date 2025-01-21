using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class GenreDtoMappingProfile : Profile
    {
        public GenreDtoMappingProfile() {
            CreateMap<Genre, GenreDto>();
        }
    }
}
