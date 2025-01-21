using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand
{
    public class AddGenreMappingProfile : Profile
    {
        public AddGenreMappingProfile()
        {
            CreateMap<AddGenreCommand, Genre>();
        }
    }
}
