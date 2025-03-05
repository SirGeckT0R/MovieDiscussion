using ApiGatewayWebAPI.ImageHandling;

namespace ApiGatewayWebAPI.Helpers
{
    public interface IRequestHelper
    {
        void DeleteOldImage(HttpContext context, ImageInfo imageInfo);
        Task<ImageInfo> ExtractImageToHeaderAsync(HttpContext context, string directory);
        Task<ImageInfo> ExtractOldImageAsync(HttpContext context);
        Task SaveImageAsync(HttpContext context, ImageInfo imageInfo);
    }
}