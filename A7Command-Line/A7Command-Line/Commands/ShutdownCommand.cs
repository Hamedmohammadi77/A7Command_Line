using System.Diagnostics;
using A7Command_Line.Commands.Abstraction;
using MyShell.Core;

namespace A7Command_Line.Commands
{
    public class ShutdownCommand : ICommand
    {
        public override string Name => "shutdown";
        public override string Description => "Shuts down the system";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/s /t 0",
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
    }
}