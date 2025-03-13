using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Commands.ManageNotApprovedMovieCommand
{
    public record ManageNotApprovedMovieCommand(Guid MovieId, bool ShouldApprove) : ICommand<Unit>;
}
