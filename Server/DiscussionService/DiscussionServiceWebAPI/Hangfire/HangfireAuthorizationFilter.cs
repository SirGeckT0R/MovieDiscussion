using Hangfire.Dashboard;

namespace DiscussionServiceWebAPI.Hangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}

