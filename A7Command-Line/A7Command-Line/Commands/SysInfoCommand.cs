using System;
using System.Runtime.InteropServices;
using A7Command_Line.Commands.Abstraction;
using MyShell.Core;


namespace A7Command_Line.Commands
{
    public class SysInfoCommand : ICommand
    {
        public override string Name => "sysinfo";
        public override string Description => "Displays system information";

        public override void Execute(string[] args, CommandContext.CommandContext context)
        {
            Console.WriteLine($"Machine Name : {Environment.MachineName}");
            Console.WriteLine($"User Name    : {Environment.UserName}");
            Console.WriteLine($"OS           : {RuntimeInformation.OSDescription}");
            Console.WriteLine($"64-bit OS    : {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"Processors   : {Environment.ProcessorCount}");
            Console.WriteLine($".NET Version : {Environment.Version}");
        }
    }
}