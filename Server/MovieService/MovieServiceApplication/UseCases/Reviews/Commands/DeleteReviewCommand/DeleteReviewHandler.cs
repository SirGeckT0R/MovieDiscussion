using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceApplication.Jobs;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand
{
    public class DeleteReviewHandler(IUnitOfWork unitOfWork, ILogger<DeleteReviewHandler> logger) : ICommandHandler<DeleteReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteReviewHandler> _logger = logger;

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (review == null)
            {
                _logger.LogError("Delete review command failed: review not found");

                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Reviews.Delete(review, cancellationToken);

            var doesJobExist = JobStorage.Current.GetConnection().GetRecurringJobs().Any(x => x.Id == $"{review.MovieId}");
            if (!doesJobExist)
            {
                RecurringJob.AddOrUpdate<CalculateRatingJob>($"{review.MovieId}", x => x.ExecuteAsync(review.MovieId), Cron.HourInterval(1));
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
