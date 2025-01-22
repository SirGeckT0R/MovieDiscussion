using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public class CreateWatchlistMappingProfile : Profile
    {
        public CreateWatchlistMappingProfile() {
            CreateMap<CreateWatchlistCommand, Watchlist>();
        }
    }
}
