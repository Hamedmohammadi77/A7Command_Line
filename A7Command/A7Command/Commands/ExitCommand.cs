namespace A7Command.Commands
{
    using ICommand = Abstraction.ICommand;

    public class ExitCommand : ICommand
    {
        public override string Name => "exit";
        public override string Description => "Exit the shell";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            context.IsRunning = false;
        }
    }
}