using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Options;
using Spectre.CommandLine;
using static System.Console;

namespace Memery.Console.Commands
{
    [Description("Prints info about the Memery CLI and settings")]
    public class InfoCommand : Command<InfoCommand.Settings>
    {
        private readonly IOptions<MemeryConsoleOptions> _options;

        public InfoCommand(IOptions<MemeryConsoleOptions> options) {
            _options = options;
        }
        public override int Execute(Settings settings, System.Linq.ILookup<string, string> unmapped)
        {
            WriteLine($"Memery {this.GetType().Assembly.GetName().Version}");
            WriteLine($"{this.GetType().Assembly.Location}: Application root is {System.IO.Directory.GetCurrentDirectory()}");
            WriteLine($"Using server at '{_options.Value.Server}'");
            WriteLine($"SSL Verification is {(_options.Value.DisableSSLVerification ? "DISABLED!" : "enabled.")}");
            return 0;
        }

        public class Settings {

        }
    }
}