namespace A7Command_Line.Commands.Abstraction
{
    public abstract class ICommand
    {
        // Name typed in shell
        public abstract string Name { get; }

        // Description shown in help
        public abstract string Description { get; }

        // Command execution logic
        public abstract void Execute(string[] args, CommandContext.CommandContext context);
    }
}