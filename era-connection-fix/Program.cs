using Fiddler;
using System.Runtime.InteropServices;

namespace era_connection_fix
{
    internal static class Program
    {
        delegate bool ConsoleCtrlDelegate(int CtrlType);

        [DllImport("Kernel32")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);

        static bool ConsoleCtrlHandler(int eventType)
        {
            switch (eventType)
            {
                case 0:
                case 1:
                case 2:
                case 5:
                case 6:
                    FiddlerCore.Stop();
                    break;
            }

            return false;
        }

        public static void Main()
        {
            SetConsoleCtrlHandler(new ConsoleCtrlDelegate(ConsoleCtrlHandler), true);

            Console.Title = "Era Connection Fix";
            Console.Clear();
            Globals.EraLogoFunc();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[!] A popup will appear saying: 'DO_NOT_TRUST_FiddlerRoot', you must click 'Yes' for this to work.\nPress any key to start...\n\n");
            Console.ResetColor();
            Console.ReadKey();
            HandleFiddler();
        }

        private static void HandleFiddler()
        {
            Globals.EnsureSelfDataFolderExists();
            Console.WriteLine("[*] Starting Proxy...");
            FiddlerCore.Start();
            FiddlerCore.GetIsRunning();
            Console.WriteLine("[+] Proxy is running, you may start Fortnite through the Era launcher...");

            FiddlerApplication.BeforeResponse += FiddlerCore.Debug;
            while (true)
            {
                Thread.Sleep(600000); //literally just to stop it closing
            }
        }
    }
}