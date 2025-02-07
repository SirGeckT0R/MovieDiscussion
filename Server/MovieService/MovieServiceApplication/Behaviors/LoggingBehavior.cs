using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MovieServiceApplication.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("----- Handling request '{Request}'", request.ToString());

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();

            var timeTaken = timer.Elapsed.TotalSeconds;
            var requestName = request.GetType();
            logger.LogInformation("----- Request '{RequestName}' handled ({TimeTaken} seconds)", requestName, timeTaken);

            return response;
        }
    }
}
