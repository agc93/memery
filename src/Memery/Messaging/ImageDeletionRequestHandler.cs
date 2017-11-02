using MediatR;
using Memery.Services;

namespace Memery.Messaging
{
    public class ImageDeletionRequestHandler : IRequestHandler<ImageDeletionRequest>
    {
        private readonly IImageIndexService _indexService;
        private readonly IFileUploadService _fileService;

        public ImageDeletionRequestHandler(IImageIndexService indexService, IFileUploadService fileService) {
            _indexService = indexService;
            _fileService = fileService;
        }
        public void Handle(ImageDeletionRequest message)
        {
            _indexService.RemoveImage(message.Image.Id);
            message.Image.SetLocation(_fileService.RootPath.FullName);
            _fileService.Delete(message.Image.FileLocation);
        }
    }
}