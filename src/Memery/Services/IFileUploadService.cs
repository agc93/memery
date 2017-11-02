using System.IO;
using Microsoft.AspNetCore.Http;

namespace Memery.Services
{
    public interface IFileUploadService
    {
        DirectoryInfo RootPath {get;}
        FileInfo Save(IFormFile file);
        FileInfo Save(System.Uri remoteFile);
        bool Delete(FileInfo file);
    }
    
}