using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand
{
    public record DeleteWatchlistCommand(Guid ProfileId) : ICommand<Unit>;
}
