namespace ApiGatewayWebAPI.ImageHandling
{
    public record ImageInfo
    {
        public string? OldFilename { get; set; }
        public string? Filename { get; set; }
        public IFormFile? File { get; set; }

        public ImageInfo()
        {
        }

        public ImageInfo(string? oldFilename, string? filename, IFormFile? file)
        {
            OldFilename = oldFilename;
            Filename = filename;
            File = file;
        }
    }
}
