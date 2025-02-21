using ApiGatewayWebAPI.ImageHandling;

namespace ApiGatewayWebAPI.Services
{
    public interface IImageService
    {
        void DeleteImage(DeleteImageRequest request);
        Task StoreImageAsync(StoreImageRequest request, CancellationToken cancellationToken);
    }
}