using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieMappingProfile : Profile
    {
        public UpdateMovieMappingProfile() {
            CreateMap<UpdateMovieCommand, Movie>();
        }
    }
}
