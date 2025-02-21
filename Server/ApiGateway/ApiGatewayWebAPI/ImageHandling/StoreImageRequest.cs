namespace ApiGatewayWebAPI.ImageHandling
{
    public class StoreImageRequest
    {
        public string FilePath { get; set; }
        public IFormFile Image { get; set; }
        public StoreImageRequest()
        {

        }

        public StoreImageRequest(string filePath, IFormFile formFile)
        {
            FilePath = filePath;
            Image = formFile;
        }
    }
}
