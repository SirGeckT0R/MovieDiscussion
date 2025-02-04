using DiscussionServiceDomain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace MovieServiceWebAPI.ExceptionHandler
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

            int status = exception switch
            {
                ValidationException or BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                NotFoundException => StatusCodes.Status404NotFound,
                OperationCanceledException => StatusCodes.Status408RequestTimeout,
                _ => StatusCodes.Status500InternalServerError
            };

            await ExceptionResponseHelper.HandleExceptionResponse(httpContext, status, exception);

            return true;
        }
    }
}
