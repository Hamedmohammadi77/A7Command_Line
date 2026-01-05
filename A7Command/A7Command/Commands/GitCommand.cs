using System;
using System.Diagnostics;
using System.Linq;
using A7Command.Commands.Abstraction;
// REQUIRED for .Skip()

namespace A7Command.Commands
{
    public class GitCommand : ICommand
    {
        public override string Name => "git";
        public override string Description => "Custom Git wrapper (save, sync, status, log, init)";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            var sub = args[0].ToLower();
            // Skip(1) needs System.Linq
            var rest = args.Skip(1).ToArray();

            switch (sub)
            {
                case "status": RunGit("status"); break;
                case "save": Save(rest); break;
                case "sync": Sync(); break;
                case "log": RunGit("log --oneline --graph --decorate"); break;
                case "init": InitRepository(); break;
                default:
                    Console.WriteLine($"Unknown git command: {sub}");
                    PrintHelp();
                    break;
            }
        }

        private void Save(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: git save \"commit message\"");
                return;
            }
            string message = string.Join(" ", args);
            RunGit("add .");
            RunGit($"commit -m \"{message}\"");
        }

        private void Sync()
        {
            Console.WriteLine("Syncing: Pulling...");
            RunGit("pull");
            Console.WriteLine("Syncing: Pushing...");
            RunGit("push");
        }

        private void InitRepository()
        {
            RunGit("init");
            Console.WriteLine("Initialized empty Git repository.");
        }

        private void RunGit(string arguments)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = arguments,
                    UseShellExecute = false, // Set to false to wait for exit properly
                    CreateNoWindow = false
                });

                // IMPORTANT: Wait for Git to finish before letting the user type again
                process?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Git: {ex.Message}. Is Git installed and in your PATH?");
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("\nGit Wrapper Commands:");
            Console.WriteLine("  git status           Show repository status");
            Console.WriteLine("  git save \"msg\"      Add all & commit with message");
            Console.WriteLine("  git sync             Pull then push");
            Console.WriteLine("  git log              Compact graph log");
            Console.WriteLine("  git init             Initialize a new repository\n");
        }
    }
}