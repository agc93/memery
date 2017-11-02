using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Memery.Messaging
{
    public class ImageUploadRequest : IRequest<ImageReference> {

        public ImageUploadRequest(IFormFile file, string name = null) {
            Upload = file;
            Name = name ?? file.FileName ?? file.Name;
        }

        public ImageUploadRequest(Uri uri, string name = null) {
            Location = uri;
            Name = name ?? System.IO.Path.GetFileName(uri.LocalPath);
        }

        public IFormFile Upload { get; }
        public string Name { get; }
        public Uri Location {get;}
    }
}