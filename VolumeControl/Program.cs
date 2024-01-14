using System;
using System.Threading;
using System.Windows.Forms;

namespace VolumeControl
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{643ab993-4353-4b9e-b7ca-5a7db25c67fe}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                TrayUtils.PostMessage((IntPtr)TrayUtils.HWND_BROADCAST, TrayUtils.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool visible = true;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1].Equals("--hide-window"))
            {
                visible = false;
            }
            Application.Run(new Form1(visible));
            mutex.ReleaseMutex();
        }
    }
}
