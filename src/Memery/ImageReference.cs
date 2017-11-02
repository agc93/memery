using System;
using System.IO;

namespace Memery
{
    public class ImageReference: ImageResponse
    {
        public ImageReference(string relativePath, string name = null)
        {
            var fi = new FileInfo(relativePath);
            Location = relativePath;
            Id = fi;
            Name = name ?? System.IO.Path.GetFileNameWithoutExtension(fi.FullName);
        }

        public ImageReference() {}
        
        /// <summary>
        /// This property is not guaranteed to be set!
        /// </summary>
        [YamlDotNet.Serialization.YamlIgnore]
        public string ContentType { get; set; }

        [YamlDotNet.Serialization.YamlIgnore]
        public FileInfo FileLocation { get; private set; }

        internal void SetLocation(string rootPath)
        {
            FileLocation = new FileInfo(Path.Combine(new DirectoryInfo(rootPath).FullName, Location));
        }
    }
}