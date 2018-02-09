using System;
using System.ComponentModel;
using static System.Console;
using Spectre.CommandLine;
using System.Linq;

namespace Memery.Console.Commands
{
    [Description("Add an image from the web")]
    public class AddRemoteImageCommand : Command<AddRemoteImageCommand.Settings>
    {
        private readonly MemeryClient _client;

        public AddRemoteImageCommand(MemeryClient client)
        {
            _client = client;
        }
        public override int Execute(Settings settings, ILookup<string, string> unmapped)
        {
            var url = Uri.TryCreate(settings.Location, UriKind.Absolute, out var u)
                ? u
                : throw new ArgumentException("Could not parse URI!");
            if (url.Scheme != "http" && url.Scheme != "https") throw new NotSupportedException("Memery only supports HTTP and HTTPS links!");
            var image = _client.AddImage(url, settings.Name).Result;
            WriteLine("Your image has been successfully uploaded and can now be reached at:");
            WriteLine($"\t{_client.BaseAddress}{image.Id}");
            WriteLine($"\t{_client.BaseAddress}{image.Name}");
            return 0;
        }

        public class Settings
        {
            [CommandArgument(0, "<url>")]
            public string Location { get; set; }

            [CommandOption("-n|--name")]
            public string Name { get; set; }
        }
    }
}