using System.IO;
using MediatR;

namespace Memery.Messaging
{
    public class ImageRequest : IRequest<ImageReference>
    {
        public ImageRequest(string id, bool detectContentType = false) {
            Id = id;
            DetectContentType = detectContentType;
        }

        public string Id { get; }
        public bool DetectContentType { get; }
    }
}