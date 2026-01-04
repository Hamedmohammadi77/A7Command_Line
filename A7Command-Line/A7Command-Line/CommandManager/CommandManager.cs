using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using A7Command_Line.CommandContext;
using ICommand = A7Command_Line.Commands.Abstraction.ICommand;

namespace MyShell.Core
{
    public class CommandManager
    {
        private readonly Dictionary<string, ICommand> _commands = new();

        public void Register(ICommand command)
        {
            _commands[command.Name.ToLower()] = command;
        }

        public void Execute(string input, CommandContext context)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            var name = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (_commands.TryGetValue(name, out var command))
            {
                command.Execute(args, context);
            }
            else
            {
                Console.WriteLine($"Unknown command: {name}");
            }
        }

        public IEnumerable<ICommand> GetAllCommands()
        {
            return _commands.Values;
        }
    }
}