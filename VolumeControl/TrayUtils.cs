using System;
using System.Runtime.InteropServices;

namespace VolumeControl
{
    public class TrayUtils
    {
        /**
         * Notify the main window and activate it
         * Copied from https://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-wpf-application#answer-522874
         */
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll")]
        public static extern int RegisterWindowMessage(string message);

        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
    }
}
