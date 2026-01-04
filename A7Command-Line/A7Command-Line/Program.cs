using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

static class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;
    const uint MOD_CTRL = 0x0002;
    const uint MOD_ALT = 0x0001;
    const int HOTKEY_ID = 1;

    [STAThread]
    static void Main()
    {
        // 1️⃣ Initialize commands
        var manager = new CommandManager();
        var context = new CommandContext();

        manager.Register(new HelpCommand(manager));
        manager.Register(new EndTaskCommand());
        manager.Register(new GitCommand());
        // ... other commands

        // 2️⃣ Hide console initially
        var consoleHandle = GetConsoleWindow();
        ShowWindow(consoleHandle, SW_HIDE);

        // 3️⃣ Register global hotkey Ctrl+Alt+M
        if (!RegisterHotKey(IntPtr.Zero, HOTKEY_ID, MOD_CTRL | MOD_ALT, (uint)Keys.M))
        {
            Console.WriteLine("Failed to register hotkey.");
        }

        // 4️⃣ Run hotkey message loop (WinForms)
        Application.Run(new HotkeyLoop(manager, context, consoleHandle));
    }
}   