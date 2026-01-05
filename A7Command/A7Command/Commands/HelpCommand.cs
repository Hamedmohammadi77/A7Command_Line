using System;
using System.Linq;
using A7Command.Commands.Abstraction;
// Added this to fix OrderBy

// Changed to match your project name

namespace A7Command.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly CommandManager.CommandManager _manager;

        public HelpCommand(CommandManager.CommandManager manager)
        {
            _manager = manager;
        }

        public override string Name => "help";
        public override string Description => "Displays all available commands";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            Console.WriteLine("\n--- Available Commands ---");

            // Fix: OrderBy requires System.Linq
            var sortedCommands = _manager.GetAllCommands().OrderBy(c => c.Name);

            foreach (var cmd in sortedCommands)
            {
                // Format output: Name (10 spaces) - Description
                Console.WriteLine($"{cmd.Name,-10} : {cmd.Description}");
            }
            Console.WriteLine("--------------------------\n");
        }
    }
}