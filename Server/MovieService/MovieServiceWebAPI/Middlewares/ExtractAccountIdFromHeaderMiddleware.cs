using System.Security.Claims;

namespace MovieServiceWebAPI.Middlewares
{
    public class ExtractAccountIdFromHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ExtractAccountIdFromHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, ILogger<ExtractAccountIdFromHeaderMiddleware> logger)
        {
            if (context.User != null)
            {
                var claim = context.Request.Headers["AccountId"].ToString();

                var apiSecret = context.Request.Headers["ApiSecret"].ToString();

                var isAccountIdEmpty = string.IsNullOrWhiteSpace(claim);
                var isApiSecretValid = apiSecret.Equals(configuration["ApiGatewaySecretKey"]);

                if (!isAccountIdEmpty && !isApiSecretValid)
                {
                    logger.LogError("Request that passed AccountId header had invalid ApiSecret header");

                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Request that passed AccountId header had invalid ApiSecret header");
                }

                var claims = new List<Claim> {
                    new Claim("AccountId", claim)
                };

                var appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);
            }

            await _next(context);
        }
    }
}
