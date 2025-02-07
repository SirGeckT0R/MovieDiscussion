using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Jobs;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.Helpers
{
    public static class CalculateRatingJobHelper
    {
        public static void AddJob(Guid movieId, IConfiguration configuration, ILogger logger)
        {
            var jobIndex = $"{movieId}";
            var doesJobExist = JobStorage.Current.GetConnection().GetRecurringJobs().Any(x => x.Id == jobIndex);

            if (!doesJobExist)
            {
                int hourInterval;
                var hasConversionSucceded = int.TryParse(configuration["Hangfire:CalculateRatingHourInterval"]!, out hourInterval);

                if (!hasConversionSucceded)
                {
                    logger.LogError("Add job to calculate rating for {Id} failed: No valid hour interval was found in configuration", movieId);

                    throw new NotFoundException("No valid hour interval was found in configuration");
                }

                var cronExpression = Cron.HourInterval(hourInterval);
                RecurringJob.AddOrUpdate<CalculateRatingJob>(jobIndex, x => x.ExecuteAsync(movieId), cronExpression);
            }
        }
    }
}
