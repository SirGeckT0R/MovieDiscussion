using ApiGatewayWebAPI.Extensions;
using ApiGatewayWebAPI.ImageHandling;
using ApiGatewayWebAPI.Services;
using System.Text.Json;
using System.Text;

namespace ApiGatewayWebAPI.Helpers
{
    public class RequestHelper(IImageService imageService, IWebHostEnvironment webHostEnvironment) : IRequestHelper
    {
        private readonly IImageService _imageService = imageService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<ImageInfo> ExtractImageToHeaderAsync(HttpContext context, string directory)
        {
            var imageInfo = new ImageInfo();

            context.Request.EnableBuffering();
            var form = await context.Request.ReadFormAsync();

            form.TryGetValue("Image", out var strings);
            if (!string.IsNullOrWhiteSpace(strings))
            {
                imageInfo.OldFilename = strings;
            }

            if (form.Files.Any() && form.Files[0].IsImage())
            {
                imageInfo.File = form.Files[0];

                var newFilename = Guid.NewGuid() + Path.GetExtension(imageInfo.File.FileName);
                var apiUrl = directory + newFilename;
                imageInfo.Filename = Path.Combine(_webHostEnvironment.WebRootPath, apiUrl);

                context.Request.Headers["Filename"] = apiUrl;
            }

            context.Request.Body.Position = 0;

            return imageInfo;
        }

        public async Task SaveImageAsync(HttpContext context, ImageInfo imageInfo)
        {
            if (string.IsNullOrWhiteSpace(imageInfo.Filename) || imageInfo.File == null)
            {
                return;
            }

            var request = new StoreImageRequest(imageInfo.Filename, imageInfo.File);
            await _imageService.StoreImageAsync(request, default);
        }

        public async Task<ImageInfo> ExtractOldImageAsync(HttpContext context)
        {
            var imageInfo = new ImageInfo();

            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            var json = JsonDocument.Parse(body);
            var hasExtractedImage = json.RootElement.TryGetProperty("image", out var imageProperty);
            if (hasExtractedImage)
            {
                string? image = imageProperty.GetString();
                if (!string.IsNullOrWhiteSpace(image))
                {
                    imageInfo.OldFilename = image;
                }
            }

            context.Request.Body.Position = 0;

            return imageInfo;
        }

        public void DeleteOldImage(HttpContext context, ImageInfo imageInfo)
        {
            if (string.IsNullOrWhiteSpace(imageInfo.OldFilename))
            {
                return;
            }

            var filepath = Path.Combine(_webHostEnvironment.WebRootPath, imageInfo.OldFilename);
            var deleteOld = new DeleteImageRequest(filepath);

            _imageService.DeleteImage(deleteOld);
        }
    }
}
