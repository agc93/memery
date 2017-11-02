namespace Memery
{
    public class ImageResponse
    {
        public ImageResponse(ShortCode c, string fileName, string filePath = null)
        {
            Id = c;
            Name = fileName;
            Location = filePath ?? string.Empty;
        }

        protected ImageResponse()
        {

        }

        public ShortCode Id { get; protected set; }
        public string Name { get; protected set; }
        /// <summary>
        /// This is a *relative* path to the configured image root
        /// </summary>
        public string Location { get; protected set; }

        public override string ToString()
        {
            return $"{Id} ({Location})";
        }
    }
}