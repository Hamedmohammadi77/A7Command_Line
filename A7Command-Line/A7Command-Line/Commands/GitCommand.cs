using System.Diagnostics;
using A7Command_Line.Commands.Abstraction;

namespace A7Command_Line.Commands
{
    public class GitCommand : ICommand
    {
        public override string Name => "git";
        public override string Description => "Custom Git wrapper";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            var sub = args[0].ToLower();
            var rest = args.Skip(1).ToArray();

            switch (sub)
            {
                case "status":
                    RunGit("status");
                    break;

                case "save":
                    Save(rest);
                    break;

                case "sync":
                    Sync();
                    break;

                case "log":
                    RunGit("log --oneline --graph --decorate");
                    break;

                case "init":  // <--- New
                    InitRepository();
                    break;

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
            RunGit("pull");
            RunGit("push");
        }

        private void InitRepository()
        {
            try
            {
                RunGit("init");
                Console.WriteLine("Initialized empty Git repository in current directory.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize repository: {ex.Message}");
            }
        }

        private void RunGit(string arguments)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            });
        }

        private void PrintHelp()
        {
            Console.WriteLine("git status           Show repository status");
            Console.WriteLine("git save \"msg\"      Add all & commit");
            Console.WriteLine("git sync             Pull then push");
            Console.WriteLine("git log              Compact log view");
            Console.WriteLine("git init             Initialize a new repository");
        }
    }
}
