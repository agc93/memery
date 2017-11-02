using MediatR;

namespace Memery.Messaging
{
    public class ImageDeletionRequest : IRequest
    {
        public ImageDeletionRequest(ImageReference img)
        {
            Image = img;
        }
        public ImageReference Image { get; }
    }
}