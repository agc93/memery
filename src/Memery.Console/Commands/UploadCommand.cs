using Spectre.CommandLine;
using System.Net.Http;
using Spectre.CommandLine.Annotations;
using System.IO;
using static System.Console;
using System.ComponentModel;

namespace Memery.Console.Commands
{
    [Description("Upload a local file to the Memery server")]
    public class UploadCommand : Command<UploadCommand.Settings>
    {
        private readonly MemeryClient _client;

        public UploadCommand(MemeryClient client) {
            _client = client;
        }

        public override int Run(Settings settings)
        {
            var file = new FileInfo(settings.FilePath);
            if (!file.Exists) {
                throw new FileNotFoundException("File not found!", settings.FilePath);
            }
            ImageResponse image = string.IsNullOrWhiteSpace(settings.Name)
                ? _client.UploadImage(file).Result
                : _client.UploadImage(file, settings.Name).Result;
            if (string.IsNullOrWhiteSpace(settings.Name)) {
                image = _client.UploadImage(file).Result;
            }
            WriteLine("Your image has been successfully uploaded and can now be reached at:");
            WriteLine($"\t{_client.BaseAddress}{image.Id}");
            WriteLine($"\t{_client.BaseAddress}{image.Name}");
            return 0;
        }

        public class Settings {
            [Argument(0, "<image>")]
            public string FilePath {get;set;}
            [Option("-n|--name")]
            public string Name {get;set;}
        }
    }
}