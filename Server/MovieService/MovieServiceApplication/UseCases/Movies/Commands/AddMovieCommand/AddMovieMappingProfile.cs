using AutoMapper;
using MovieServiceDomain.Models;
namespace MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand
{
    public class AddMovieMappingProfile : Profile
    {
        public AddMovieMappingProfile() {
            CreateMap<AddMovieCommand, Movie>();
        }
    }
}
