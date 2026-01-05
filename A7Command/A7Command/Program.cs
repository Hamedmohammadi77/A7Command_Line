using System;
using System.Runtime.InteropServices;
using System.Threading;
using A7Command.Commands;

namespace A7Command
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        static extern bool GetMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int HOTKEY_ID = 9000; // Unique ID
        const uint MOD_CTRL = 0x0002;
        const uint MOD_ALT = 0x0001;
        const uint VK_M = 0x4D; // M key
        const int WM_HOTKEY = 0x0312;

        [StructLayout(LayoutKind.Sequential)]
        struct NativeMessage {
            public IntPtr handle; public uint message; public IntPtr wParam;
            public IntPtr lParam; public uint time; public int pt_x; public int pt_y;
        }

        static bool isVisible = false;

        [STAThread]
        static void Main()
        {
            var manager = new A7Command.CommandManager.CommandManager();
            var context = new A7Command.CommandContext.CommandContext();

            manager.Register(new HelpCommand(manager));
            manager.Register(new GitCommand());
            manager.Register(new EndTaskCommand());
            manager.Register(new AppsCommand());

            IntPtr consoleHandle = GetConsoleWindow();

            // 1. Start Hotkey Thread
            Thread hotkeyThread = new Thread(() =>
            {
                // Try to register. If it returns false, the keys are already taken by another app!
                bool success = RegisterHotKey(IntPtr.Zero, HOTKEY_ID, MOD_CTRL | MOD_ALT, VK_M);
                
                if (!success) {
                    // This will beep 3 times quickly if the hotkey is ALREADY IN USE by another app
                    for(int i=0; i<3; i++) { Console.Beep(1000, 200); Thread.Sleep(100); }
                }

                NativeMessage msg;
                while (GetMessage(out msg, IntPtr.Zero, 0, 0))
                {
                    if (msg.message == WM_HOTKEY)
                    {
                        // BEEP when keys are pressed so we know it works!
                        Console.Beep(500, 100); 

                        isVisible = !isVisible;
                        if (isVisible) {
                            ShowWindow(consoleHandle, SW_SHOW);
                            SetForegroundWindow(consoleHandle); // Force window to front
                        } else {
                            ShowWindow(consoleHandle, SW_HIDE);
                        }
                    }
                }
            });
            hotkeyThread.IsBackground = true;
            hotkeyThread.Start();

            // 2. Hide initially
            ShowWindow(consoleHandle, SW_HIDE);

            // 3. Command Loop
            while (true)
            {
                // We must run the loop even if not visible to keep the process alive
                if (isVisible)
                {
                    Console.Write("\nA7 > ");
                    string input = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        if (input.ToLower() == "exit") break;
                        manager.Execute(input, context);
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }
    }
}