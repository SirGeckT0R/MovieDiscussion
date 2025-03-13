using ApiGatewayWebAPI.ImageHandling;
using ApiGatewayWebAPI.Enums;
using ApiGatewayWebAPI.Helpers;
using static ApiGatewayWebAPI.Helpers.HttpContextHelper;

namespace ApiGatewayWebAPI.Middlewares
{
    public class ImageHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;

        public async Task InvokeAsync(HttpContext context, IRequestHelper requestHelper)
        {
            var isMoviesRequest = context.Request.Path.StartsWithSegments(_configuration["MoviesRequestPath"]);

            if (!isMoviesRequest)
            {
                await _next(context);

                return;
            }

            var requestMethod = GetRequestMethod(context);

            switch (requestMethod)
            {
                case RequestMethod.POST when (context.Request.HasFormContentType):
                    await HandlePostAsync(context, requestHelper);

                    break;

                case RequestMethod.PUT when (context.Request.HasFormContentType):
                    await HandlePutAsync(context, requestHelper);

                    break;

                case RequestMethod.DELETE:
                    await HandleDeleteAsync(context, requestHelper);

                    break;

                default:
                    await _next(context);

                    return;
            }
        }

        private async Task<ImageInfo> HandlePostAsync(HttpContext context, IRequestHelper requestHelper)
        {
            ImageInfo imageInfo;
            imageInfo = await requestHelper.ExtractImageToHeaderAsync(context, _configuration["MovieImagesDirectory"]!);

            await _next(context);

            if (IsSuccess(context))
            {
                await requestHelper.SaveImageAsync(context, imageInfo);
            }

            return imageInfo;
        }

        private async Task<ImageInfo> HandlePutAsync(HttpContext context, IRequestHelper requestHelper)
        {
            ImageInfo imageInfo;
            imageInfo = await requestHelper.ExtractImageToHeaderAsync(context, _configuration["MovieImagesDirectory"]!);

            await _next(context);

            if (IsSuccess(context))
            {
                requestHelper.DeleteOldImage(context, imageInfo);

                await requestHelper.SaveImageAsync(context, imageInfo);
            }

            return imageInfo;
        }

        private async Task<ImageInfo> HandleDeleteAsync(HttpContext context, IRequestHelper requestHelper)
        {
            ImageInfo imageInfo;
            imageInfo = await requestHelper.ExtractOldImageAsync(context);

            await _next(context);

            if (IsSuccess(context))
            {
                requestHelper.DeleteOldImage(context, imageInfo);
            }

            return imageInfo;
        }
    }
}
