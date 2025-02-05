using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Helpers;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand
{
    public class DeleteReviewHandler(IUnitOfWork unitOfWork, ILogger<DeleteReviewHandler> logger, IConfiguration configuration) : ICommandHandler<DeleteReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteReviewHandler> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (review == null)
            {
                _logger.LogError("Delete review command failed for {Id}: review not found", request.Id);

                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Reviews.Delete(review, cancellationToken);

            CalculateRatingJobHelper.AddJob(review.MovieId, _configuration, _logger);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
