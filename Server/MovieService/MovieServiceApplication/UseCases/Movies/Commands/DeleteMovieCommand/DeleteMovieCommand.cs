using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Commands.DeleteMovieCommand
{
    public record DeleteMovieCommand(Guid Id) : ICommand<Unit>;
}
