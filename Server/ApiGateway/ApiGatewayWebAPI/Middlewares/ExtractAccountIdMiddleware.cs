namespace ApiGatewayWebAPI.Middlewares
{
    public class ExtractAccountIdMiddleware
    {
        private readonly RequestDelegate _next;

        public ExtractAccountIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var claim = context.User.FindFirst("UserId")?.Value;
            context.Request.Headers["AccountId"] = claim;

            await _next(context);
        }
    }
}
