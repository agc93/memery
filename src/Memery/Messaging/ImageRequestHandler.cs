using HeyRed.Mime;
using MediatR;
using Memery.Configuration;
using Memery.Services;
using Microsoft.Extensions.Options;
using System.IO;

namespace Memery.Messaging
{
    public class ImageRequestHandler : IRequestHandler<ImageRequest, ImageReference>
    {
        public ImageRequestHandler(IImageIndexService service, IOptions<MemeryOptions> options) {
            ImageRoot = options.Value.ImageSavePath;
            Index = service;
            var localPath = Path.Combine(Directory.GetCurrentDirectory(), "magic.mgc");
            if (File.Exists(localPath)) { // for SCD compatibility
                MimeGuesser.MagicFilePath = localPath;
            }

        }

        private string ImageRoot { get; }
        public IImageIndexService Index { get; }

        public ImageReference Handle(ImageRequest message)
        {
            var imageRef = ShortCode.TryParse(message.Id, out var c)
                ? Index.GetImage(c)
                : Index.GetImageByName(message.Id);
            imageRef.SetLocation(ImageRoot);
            if (message.DetectContentType) {
                imageRef.ContentType = MimeGuesser.GuessMimeType(imageRef.FileLocation);
            }
            
            return imageRef;
        }
    }
}