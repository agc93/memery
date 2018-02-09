using System.Linq;
using Spectre.CommandLine;

namespace Memery.Console.Commands
{
    public class BackupCommand : Command<BackupCommand.Settings>
    {
        public override int Execute(Settings settings, ILookup<string, string> remaining)
        {
            throw new System.NotImplementedException();
        }

        public class Settings {
            
        }
    }
}