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

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User != null)
            {
                var claim = context.Request.Headers["AccountId"].ToString();
                var claims = new List<Claim>
                                            {
                                                new Claim("AccountId", claim)
                                            };

                var appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);
            }

            await _next(context);
        }
        
    }
}
