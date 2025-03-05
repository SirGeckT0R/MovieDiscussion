namespace ApiGatewayWebAPI.ImageHandling
{
    public record StoreImageRequest(string FilePath, IFormFile Image);
}
