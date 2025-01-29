using MediatR;
using MovieServiceApplication.Enums;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand
{
    public record ManageMovieInWatchlistCommand : ICommand<Unit>
    {
        public Guid AccountId { get; set; }
        public Guid MovieId { get; set; }
        public WatchlistAction Action { get; set; }

        public ManageMovieInWatchlistCommand() { }
    }
}
