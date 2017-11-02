using System.ComponentModel;
using static System.Console;
using Spectre.CommandLine;
using Memery.Console.Formatting;
using Spectre.CommandLine.Annotations;
using System.Linq;

namespace Memery.Console.Commands
{
    [Description("List the images available in your collection")]
    public class ListCommand : Command<ListCommand.Settings>
    {
        private readonly MemeryClient _client;

        public ListCommand(MemeryClient client)
        {
            _client = client;
        }

        public override int Run(Settings settings)
        {
            var index = _client.GetImages().Result;
            var table = new ConsoleTable("ID", "Name", "File Name");
            ConsoleTable
                .From<ImageResponse>(settings.LinkOutput ? index.Select(Prefix) : index)
                .Write(Format.Alternative);
            // table.AddRow(image.Id, image.Name, image.Location);
            WriteLine();
            return 0;
        }

        public class Settings
        {
            [Option("-u|--url")]
            [Description("Outputs full URLs rather than just the path fragments.")]
            public bool LinkOutput { get; set; }
        }

        private ImageResponse Prefix(ImageResponse image) {
            image.Id = _client.BaseAddress + image.Id;
            image.Name = _client.BaseAddress + image.Name;
            return image;
        }
    }
}