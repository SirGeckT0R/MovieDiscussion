using Hangfire.Common;
using Hangfire.Server;
using Hangfire;
using Hangfire.Storage;

namespace MovieServiceWebAPI
{
    public class RemoveAfterFirstExecutionFilter : JobFilterAttribute, IServerFilter
    {
        public void OnPerforming(PerformingContext filterContext)
        {
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (filterContext.Connection == null) return;

            var recurringJobId = filterContext.BackgroundJob?.Id;

            if (string.IsNullOrEmpty(recurringJobId)) return;

            var recurringJobIds = filterContext.Connection.GetRecurringJobs()
                                            .Select(job => job.Id)
                                            .ToList();
            var jobData = filterContext.Connection.GetAllEntriesFromHash($"recurring-job:{recurringJobId}");
            // Find the matching recurring job
            //var recurringJob = recurringJobs.FirstOrDefault(job => job.LastJobId == jobId);

            //if (recurringJob != null && recurringJob.TryGetValue("Id", out var jobId))
            //{
            //    // Remove the job after its first execution
            //    RecurringJob.RemoveIfExists(jobId);
            //    Console.WriteLine($"Removed Recurring Job: {jobId}");
            //}
        }
        
    }

}
