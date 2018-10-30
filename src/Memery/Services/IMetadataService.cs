using Microsoft.AspNetCore.Mvc;

namespace Memery.Services
{
    public interface IMetadataService
    {
        string GetMetadataFilePath();
        string Version {get;}
        string RuntimeVersion {get;}
        IActionResult GetAppInfo();
    }
}