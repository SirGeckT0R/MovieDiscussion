using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand
{
    public class UpdateGenreMappingProfile : Profile
    {
        public UpdateGenreMappingProfile()
        {
            CreateMap<UpdateGenreCommand, Genre>();
        }
    }
}
