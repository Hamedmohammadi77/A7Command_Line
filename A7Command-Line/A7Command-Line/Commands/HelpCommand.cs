using A7Command_Line.Commands.Abstraction;
using MyShell.Core;

namespace A7Command_Line.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly CommandManager _manager;

        // Constructor: receives CommandManager to list all commands
        public HelpCommand(CommandManager manager)
        {
            _manager = manager;
        }

        // Name used in shell
        public override string Name => "help";

        // Description shown in help
        public override string Description => "Displays all available commands";

        // Execution logic
        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            Console.WriteLine("Available commands:\n");

            // List all commands alphabetically
            foreach (var cmd in _manager.GetAllCommands().OrderBy(c => c.Name))
            {
                Console.WriteLine($"{cmd.Name,-10} - {cmd.Description}");
            }
        }
    }
}