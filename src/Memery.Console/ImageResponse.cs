namespace Memery.Console
{
    public class ImageResponse
    {

        public string Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// This is a *relative* path to the configured image root
        /// </summary>
        public string Location { get; set; }

        public override string ToString()
        {
            return $"{Id} ({Location})";
        }
    }
}