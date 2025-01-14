using FluentValidation;
using FluentValidation.Results;

namespace UserServiceWebAPI.ExceptionHandler
{
    public static class ExceptionResponseHelper
    {
        public static async Task HandleExceptionResponse(HttpContext httpContext, int status, Exception exception)
        {
            var problemDetails = new
            {
                Status = status,
                Detail = exception.Message,
                Errors = GetErrors(exception)
            };

            httpContext.Response.StatusCode = status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private static List<ValidationFailure>? GetErrors(Exception exception)
        {
            List<ValidationFailure>? errors = null;
            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors.ToList();
            }
            return errors;
        }
    }
}
