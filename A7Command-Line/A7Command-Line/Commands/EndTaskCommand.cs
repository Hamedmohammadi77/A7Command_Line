using System;
using System.Diagnostics;
using System.Linq;
using A7Command_Line.Commands.Abstraction;

namespace A7Command_Line.Commands
{
    public class EndTaskCommand : ICommand
    {
        public override string Name => "end";
        public override string Description => "Terminates a process by PID or name";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: endtask <pid|processName> [-f]");
                return;
            }

            bool force = args.Contains("-f");
            string target = args[0];

            // Case 1: PID
            if (int.TryParse(target, out int pid))
            {
                KillByPid(pid, force);
                return;
            }

            // Case 2: Process name
            KillByName(target, force);
        }

        private void KillByPid(int pid, bool force)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                process.Kill(force);
                Console.WriteLine($"Process {process.ProcessName} (PID {pid}) terminated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void KillByName(string name, bool force)
        {
            var processes = Process.GetProcessesByName(name);

            if (processes.Length == 0)
            {
                Console.WriteLine($"No process found with name '{name}'.");
                return;
            }

            foreach (var process in processes)
            {
                try
                {
                    process.Kill(force);
                    Console.WriteLine($"Process {process.ProcessName} (PID {process.Id}) terminated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to kill {process.ProcessName} (PID {process.Id}): {ex.Message}");
                }
            }
        }
    }
}