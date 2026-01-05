using System.Diagnostics;
using A7Command.Commands.Abstraction;

namespace A7Command.Commands
{
    public class RestartCommand : ICommand
    {
        public override string Name => "restart";
        public override string Description => "Restarts the system";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/r /t 0",
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
    }
}