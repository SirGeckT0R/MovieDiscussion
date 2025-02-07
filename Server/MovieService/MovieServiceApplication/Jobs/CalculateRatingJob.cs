using DnsClient.Internal;
using Hangfire;
using Microsoft.Extensions.Logging;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.Jobs
{
    public class CalculateRatingJob(IUnitOfWork unitOfWork, ILogger<CalculateRatingJob> logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CalculateRatingJob> _logger = logger;

        public async Task ExecuteAsync(Guid movieId)
        {
            _logger.LogInformation("Background job to calculate movie rating with id {Id} started", movieId);

            var movie = await _unitOfWork.Movies.GetByIdTrackingAsync(movieId, default);

            if (movie == null) 
            {
                _logger.LogError("Background job to calculate movie rating with id {Id} failed: movie not found", movieId);

                throw new NotFoundException("Movie not found");
            }

            var specification = new ReviewsByMovieIdSpecification(movieId);
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(specification, default);

            if (reviews == null || reviews.Count == 0)
            {
                movie.Rating = 0;
            }
            else
            {
                var newRating = reviews.Average(r => r.Value);
                movie.Rating = newRating;
            }

            _unitOfWork.Movies.Update(movie, default);

            await _unitOfWork.SaveChangesAsync(default);

            _logger.LogInformation("Background job to calculate movie rating with id {Id} completed successfully", movieId);

            RecurringJob.RemoveIfExists(movieId.ToString());
        }
    }
}
