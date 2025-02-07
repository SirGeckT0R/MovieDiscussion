using Serilog;

namespace DiscussionServiceWebAPI.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            hostBuilder.UseSerilog((context, config) =>
            {
                config.WriteTo.Console();
                config.WriteTo.Http(configuration["LogstashUrl"]!, queueLimitBytes: null);
            });

            return hostBuilder;
        }
    }
}
