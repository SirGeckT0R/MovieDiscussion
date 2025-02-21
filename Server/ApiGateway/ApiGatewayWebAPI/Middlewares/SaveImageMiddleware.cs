using ApiGatewayWebAPI.Extensions;
using ApiGatewayWebAPI.ImageHandling;
using ApiGatewayWebAPI.Services;

namespace ApiGatewayWebAPI.Middlewares
{
    public class SaveImageMiddleware
    {
        private readonly RequestDelegate _next;
        private string _filename;
        private IFormFile file;

        public SaveImageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            if (context.Request.Path == "/api/movies" && context.Request.Method == "POST" && context.Request.HasFormContentType)
            {
                context.Request.EnableBuffering();
                var form = await context.Request.ReadFormAsync();

                if (form.Files.Any() && form.Files[0].IsImage())
                {
                    file = form.Files[0];
                }
                var apiUrl = "images/movies/" + $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                _filename = Path.Combine(webHostEnvironment.WebRootPath, apiUrl);

                context.Request.Headers["Filename"] = apiUrl;
                context.Request.Body.Position = 0;

            }
            await _next(context);

            if (context.Request.Path == "/api/movies" && context.Request.Method == "POST" && context.Response.StatusCode < 300)
            {
                var request = new StoreImageRequest(_filename, file);
                await imageService.StoreImageAsync(request, default);

            }
        }
    }
}
