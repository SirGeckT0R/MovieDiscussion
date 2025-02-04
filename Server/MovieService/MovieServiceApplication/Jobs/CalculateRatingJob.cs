using Hangfire;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.Jobs
{
    public class CalculateRatingJob(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task ExecuteAsync(Guid movieId)
        {
            var movie = await _unitOfWork.Movies.GetByIdTrackingAsync(movieId, default);

            if (movie == null) {
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

            RecurringJob.RemoveIfExists(movieId.ToString());
        }
    }
}
