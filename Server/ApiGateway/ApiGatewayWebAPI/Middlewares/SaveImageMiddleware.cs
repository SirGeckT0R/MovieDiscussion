using ApiGatewayWebAPI.Extensions;
using ApiGatewayWebAPI.ImageHandling;
using ApiGatewayWebAPI.Services;
using Microsoft.Extensions.Primitives;

namespace ApiGatewayWebAPI.Middlewares
{
    public class SaveImageMiddleware
    {
        private readonly RequestDelegate _next;
        private string? _filename;
        private string? _oldFilename;
        private IFormFile? file;

        public SaveImageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            var isMoviesRequest = context.Request.Path.StartsWithSegments("/api/movies");
            //var isDelete =  context.Request.Method == "DELETE";
            var isPostOrPut = context.Request.Method == "POST" || context.Request.Method == "PUT";
            //if(isDelete)
            //{
            //    context.Request.EnableBuffering(); 
            //    var routeId = context.Request.RouteValues["Id"];
            //}

            if (isMoviesRequest && isPostOrPut && context.Request.HasFormContentType)
            {
                context.Request.EnableBuffering();
                var form = await context.Request.ReadFormAsync();

                StringValues strings;
                form.TryGetValue("Image", out strings);
                if (!string.IsNullOrWhiteSpace(strings))
                {
                    _oldFilename = strings;
                }

                if (form.Files.Any() && form.Files[0].IsImage())
                {
                    file = form.Files[0];
                    var apiUrl = "images/movies/" + $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    _filename = Path.Combine(webHostEnvironment.WebRootPath, apiUrl);

                    context.Request.Headers["Filename"] = apiUrl;
                    context.Request.Body.Position = 0;
                }
            }

            await _next(context);

            if (isMoviesRequest && isPostOrPut && context.Response.StatusCode < 300)
            {
                if(context.Request.Method == "PUT" && !string.IsNullOrWhiteSpace(_oldFilename))
                {
                    var deleteOld = new DeleteImageRequest(Path.Combine(webHostEnvironment.WebRootPath, _oldFilename));
                    imageService.DeleteImage(deleteOld);
                }

                if (!string.IsNullOrWhiteSpace(_filename))
                {
                    var request = new StoreImageRequest(_filename, file);
                    await imageService.StoreImageAsync(request, default);
                }

            }
            _oldFilename = null;
            _filename = null;
            file = null;
        }
    }
}
