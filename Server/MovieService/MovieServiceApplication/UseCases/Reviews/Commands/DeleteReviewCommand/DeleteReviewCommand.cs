using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand
{
    public record DeleteReviewCommand(Guid Id) : ICommand<Unit>;
}
