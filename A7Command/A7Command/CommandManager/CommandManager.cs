using System;
using System.Collections.Generic;
using System.Linq;
using ICommand = A7Command.Commands.Abstraction.ICommand;

namespace A7Command.CommandManager
{
    public class CommandManager
    {
        private readonly Dictionary<string, ICommand> _commands = new();

        public void Register(ICommand command)
        {
            _commands[command.Name.ToLower()] = command;
        }

        public void Execute(string input, CommandContext.CommandContext context)
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