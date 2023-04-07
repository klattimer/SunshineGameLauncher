using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SunshineGameLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Process gameProcess;
        static bool exitSystem = false;

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Trace.WriteLine("Exiting game to external CTRL-C, or process kill, or shutdown");

            gameProcess.CloseMainWindow();
            //do your cleanup here
            Thread.Sleep(5000); //simulate some cleanup delay

            Trace.WriteLine("Cleanup complete");

            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(0);

            return true;
        }
        #endregion

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 2)
            {
                Trace.WriteLine("ERROR: Needs launch URL and EXE Name");
                return;
            }
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            MainWindow wnd = new MainWindow();
            wnd.Show();
            Thread.Sleep(1000);
            wnd.Topmost = false;
            Execute(e.Args[0], e.Args[1]);
        }

        public void Execute(string url, string binary_name)
        {

            var ps = new ProcessStartInfo(url)
            {
                UseShellExecute = true,
                Verb = "open"
            };

            Trace.WriteLine($"Starting url: {url}");
            Process.Start(ps);
            Trace.WriteLine($"Looking for exe: {binary_name}");
            Process[] gameProcesses = null;
            for (var i = 0; i < 60; i++)
            {
                Thread.Sleep(1000);
                gameProcesses = Process.GetProcessesByName(binary_name);
                if (gameProcesses.Length > 0)
                {
                    break;
                }
            }

            if (gameProcesses == null || gameProcesses.Length == 0)
            {
                Trace.WriteLine($"Could not find process with name: {binary_name}");
                Environment.Exit(-1);
            }
            gameProcess = gameProcesses[0];

            Trace.WriteLine($"Game started.");
            gameProcesses[0].WaitForExit();
            Trace.WriteLine($"Game exited.");
            Trace.WriteLine($"Closing launcher.");
            Environment.Exit(0);
        }
    }
}
