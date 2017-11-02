using MediatR;
using Memery.Services;

namespace Memery.Messaging
{
    public class ImageUploadRequestHandler : IRequestHandler<ImageUploadRequest, ImageReference>
    {
        public ImageUploadRequestHandler(IFileUploadService uploadService, IImageIndexService indexService) {
            UploadService = uploadService;
            IndexService = indexService;
        }

        public IFileUploadService UploadService { get; }
        public IImageIndexService IndexService { get; }

        public ImageReference Handle(ImageUploadRequest message)
        {
            var file = message.Upload != null
                ? UploadService.Save(message.Upload)
                : UploadService.Save(message.Location);
            var imgCode = IndexService.AddImage(System.IO.Path.GetRelativePath(UploadService.RootPath.FullName, file.FullName), message.Name);
            return IndexService.GetImage(imgCode);
        }
    }
}