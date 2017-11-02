using System;
using System.IO;
using System.Net.Http;
using Memery.Configuration;
using Memery.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Memery.Services
{
    public class FileUploadService : IFileUploadService
    {
        public FileUploadService(IOptions<MemeryOptions> options)
        {
            var path = options.Value.ImageSavePath;
            RootPath = new DirectoryInfo(path);
            _overwrite = options.Value.OverwriteFiles;
            if (!RootPath.Exists)
            {
                Directory.CreateDirectory(RootPath.FullName);
            }
        }

        public DirectoryInfo RootPath { get; }

        private readonly bool _overwrite;

        public bool Delete(FileInfo file)
        {
            if (!file.Exists) {
                return true;
            }
            try {
                File.Delete(file.FullName);
                file.Refresh();
                return !file.Exists;
            } catch {
                return false;
            }
        }

        public FileInfo Save(IFormFile file)
        {
            if (!IsValid(file.FileName))
            {
                throw new InvalidFileNameException(file.FileName);
            }
            var filePath = GetFilePath(file.FileName);
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath.FullName, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return filePath;
        }

        public FileInfo Save(Uri remoteFile)
        {
            if (!remoteFile.IsAbsoluteUri) throw new ArgumentException(nameof(remoteFile));
            var fileName = Path.GetFileName(remoteFile.LocalPath);
            if (!IsValid(fileName))
            {
                throw new InvalidFileNameException(fileName);
            }
            var filePath = GetFilePath(fileName);
            using (var client = new HttpClient()) {
                var response = client.GetAsync(remoteFile).Result;
                using (var stream = new FileStream(filePath.FullName, FileMode.Create)) {
                    response.Content.CopyToAsync(stream).Wait();
                }
            }
            return filePath;
        }

        private FileInfo GetFilePath(string fileName)
        {
            return _overwrite
                ? new FileInfo(Path.Combine(RootPath.FullName, fileName))
                : new FileInfo(Path.Combine(
                    RootPath.FullName,
                    $"{Path.GetFileNameWithoutExtension(fileName)}.{DateTime.Now.Ticks}.{Path.GetExtension(fileName)}"));
        }

        private bool IsValid(string fileName)
        {
            System.IO.FileInfo fi = null;
            try
            {
                fi = new System.IO.FileInfo(fileName);
            }
            catch (System.ArgumentException) { }
            catch (System.IO.PathTooLongException) { }
            catch (System.NotSupportedException) { }
            return 
                !ReferenceEquals(fi, null) &&
                fileName.IndexOfAny(Path.GetInvalidFileNameChars()) <= 0;
        }
    }
}