using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand
{
    public class DeleteReviewHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Review not found");

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Reviews.Delete(review, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
