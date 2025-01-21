using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand
{
    public record AddGenreCommand(string Name) : ICommand<Unit>;
}
