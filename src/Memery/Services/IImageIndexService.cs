using System.Collections.Generic;
using System.IO;

namespace Memery.Services
{
    public interface IImageIndexService
    {
        Dictionary<string, ImageReference> GetIndex();
        ImageReference GetImage(ShortCode c);
        ImageReference GetImageByName(string name);
        ShortCode AddImage(string filePath, string name = null);
        bool RemoveImage(ShortCode code);
    }
}