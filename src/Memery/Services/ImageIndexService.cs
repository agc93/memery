using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Memery.Configuration;
using Memery.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Memery.Services
{
    public class ImageIndexService : IImageIndexService
    {
        private readonly DirectoryInfo _imageRoot;

        public ImageIndexService(IOptions<MemeryOptions> options)
        {
            var name = Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                options.Value.IndexFilePath
            );
            var fi = new FileInfo(name);
            _imageRoot = new DirectoryInfo(options.Value.ImageSavePath);
            if (!fi.Exists)
            {
                if (!fi.Directory.Exists) {
                    fi.Directory.Create();
                }
                File.Create(name).Dispose();
            }
            Index = fi;
        }
        private FileInfo Index { get; }
        public ImageReference GetImage(ShortCode c)
        {
            var index = GetIndex();
            if (index.ContainsKey(c))
            {
                return index[c];
            }
            else
            {
                throw new ImageNotFoundException();
            }
        }

        public ImageReference GetImageByName(string name) {
            var index = GetIndex();
            var images = index.Where(i => i.Value.Name == name);
            var image = images.Any() ? images.Random().Value : null;
            return image == null
                ? throw new ImageNotFoundException()
                : image;
        }

        public ShortCode AddImage(string filePath, string name = null)
        {
            var imgRef = new ImageReference(filePath, name);
            var index = GetIndex();
            if (!index.ContainsKey(imgRef.Id)) {
                index.Add(imgRef.Id, imgRef);
            }
            UpdateIndex(index);
            return imgRef.Id;
        }

        public Dictionary<string, ImageReference> GetIndex()
        {
            var deser = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTypeConverter(new ShortCodeConverter())
                .Build();
            var index = deser.Deserialize<Dictionary<string, ImageReference>>(File.ReadAllText(Index.FullName));
            return index ?? new Dictionary<string, ImageReference>();
        }

        private bool UpdateIndex(Dictionary<string, ImageReference> index)
        {
            try
            {
                var ser = new SerializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .WithTypeConverter(new ShortCodeConverter())
                    .Build();
                var i = ser.Serialize(index);
                System.IO.File.WriteAllText(Index.FullName, i);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveImage(ShortCode code)
        {
            var index = GetIndex();
            if (index.ContainsKey(code)) {
                index.Remove(code);
            }
            return UpdateIndex(index);
        }

        internal class ShortCodeConverter : YamlDotNet.Serialization.IYamlTypeConverter
        {
            public bool Accepts(Type type)
            {
                return type == typeof(ShortCode);
            }

            public object ReadYaml(IParser parser, Type type)
            {
                var value = ((Scalar)parser.Current).Value;
                parser.MoveNext();
                return ShortCode.TryParse(value, out var c)
                    ? c
                    : null;
            }

            public void WriteYaml(IEmitter emitter, object value, Type type)
            {
                if (value != null) {
                    emitter.Emit(new Scalar((string)((ShortCode)value)));
                }
            }
        }
    }
}