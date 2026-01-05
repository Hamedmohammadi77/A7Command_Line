using System;
using System.Diagnostics;
using System.Linq;
using A7Command.Commands.Abstraction;

namespace A7Command.Commands
{
    public class EndTaskCommand : ICommand
    {
        public override string Name => "end";
        public override string Description => "Terminates a process by PID or Name (Usage: end <target> [-f])";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: end <pid|processName> [-f]");
                return;
            }

            // Check if user wants to force (kill process tree)
            bool force = args.Contains("-f");
            string target = args[0];

            // Case 1: If input is a number, kill by PID
            if (int.TryParse(target, out int pid))
            {
                KillByPid(pid, force);
            }
            // Case 2: Otherwise, kill by Name
            else
            {
                KillByName(target, force);
            }
        }

        private void KillByPid(int pid, bool force)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                string pName = process.ProcessName;
                process.Kill(force); // force = true kills child processes too
                Console.WriteLine($"Successfully terminated {pName} (PID: {pid}).");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Error: Process with PID {pid} was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Could not kill PID {pid}. {ex.Message}");
            }
        }

        // Overload for the name kill with force flag
        private void KillByName(string name, bool force)
        {
            var processes = Process.GetProcessesByName(name);

            if (processes.Length == 0)
            {
                Console.WriteLine($"No running processes found named '{name}'.");
                return;
            }

            foreach (var process in processes)
            {
                try
                {
                    process.Kill(force);
                    Console.WriteLine($"Terminated {name} (PID: {process.Id}).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to kill {name}: {ex.Message}");
                }
            }
        }
    }
}