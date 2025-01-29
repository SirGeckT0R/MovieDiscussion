using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public record CreateWatchlistCommand(Guid AccountId) : ICommand<Unit>;
}
