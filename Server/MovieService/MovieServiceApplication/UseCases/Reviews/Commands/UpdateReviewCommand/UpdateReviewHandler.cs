using AutoMapper;
using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceApplication.Jobs;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateReviewHandler> logger) : ICommandHandler<UpdateReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateReviewHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (review == null)
            {
                _logger.LogError("Update review command failed for {Id}: review not found", request.Id);

                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var newReview = _mapper.Map(request, review);
            _unitOfWork.Reviews.Update(newReview, cancellationToken);

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
