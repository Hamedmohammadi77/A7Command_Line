using System;
using System.Diagnostics;
using A7Command.Commands.Abstraction;

namespace A7Command.Commands
{
    public class AppsCommand : ICommand
    {
        public override string Name => "apps";
        public override string Description => "Lists running applications";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            foreach (var process in Process.GetProcesses())
            {
                if (!string.IsNullOrWhiteSpace(process.MainWindowTitle))
                {
                    Console.WriteLine($"{process.ProcessName}  (PID: {process.Id})");
                }
            }
        }
    }
}