using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Genres.Commands.DeleteGenreCommand
{
    public record DeleteGenreCommand(Guid Id) : ICommand<Unit>;
}
