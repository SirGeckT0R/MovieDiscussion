using ApiGatewayWebAPI.ImageHandling;

namespace ApiGatewayWebAPI.Services
{
    public class ImageService : IImageService
    {
        public async Task StoreImageAsync(StoreImageRequest request, CancellationToken cancellationToken)
        {
            if (request.Image == null)
            {
                return;
            }

            using (var stream = new FileStream(request.FilePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream, cancellationToken);
            }
        }

        public void DeleteImage(DeleteImageRequest request)
        {
            FileInfo file = new FileInfo(request.Path);

            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
