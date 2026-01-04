using A7Command_Line.Commands;
using MyShell.Core;

namespace A7Command_Line;

class Program
{
    static void Main()
    {
        string consoleName = "A7Command_Line";

        var context = new CommandContext.CommandContext();
        var manager = new CommandManager();

        manager.Register(new ExitCommand());
        manager.Register(new AppsCommand());
        manager.Register(new RestartCommand());
        manager.Register(new ShutdownCommand());
        manager.Register(new SysInfoCommand());
        manager.Register(new EndTaskCommand());
        manager.Register(new GitCommand());
        manager.Register(new HelpCommand(manager));


        Console.Title = $"{consoleName}";

        while (context.IsRunning)
        {
            Console.Write($"{consoleName}> ");
            var input = Console.ReadLine();
            if (input != null)
            {
                manager.Execute(input, context);
            }
        }
    }
}