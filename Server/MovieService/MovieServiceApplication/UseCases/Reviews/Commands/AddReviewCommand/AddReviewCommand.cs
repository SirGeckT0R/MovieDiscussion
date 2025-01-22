using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand
{
    public record AddReviewCommand(Guid UserId, Guid MovieId, int Value, string Text) : ICommand<Unit>;
}
