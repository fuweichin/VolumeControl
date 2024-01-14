using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace VolumeControl
{
    /**
     * extract icons from a DLL
     * derived from https://stackoverflow.com/a/6873026/2189544
     */
    public class IconExtractor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">e.g. "%windir%/system32/shell32.dll,-47"</param>
        /// <returns>path, index</returns>
        public static Tuple<string, int> ParsePath(string input)
        {
            string path = "";
            int index = 0;
            string[] arr = input.Split(',');
            if (arr.Length == 2 && Regex.Match(arr[1], @"(-?\d+)$").Success)
            {
                index = Convert.ToInt32(arr[1]);
            }
            path = Regex.Replace(arr[0], @"%(\w+)%", (m) =>
            {
                return Environment.GetEnvironmentVariable(m.Groups[1].Value);
            }, RegexOptions.ECMAScript);
            return new Tuple<string, int>(path, index);
        }
        // large 32x32, small 16x16
        public static Icon Extract(string filePath, int index, bool largeIcon = true)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            IntPtr hIcon;
            if (largeIcon)
            {
                ExtractIconEx(filePath, index, out hIcon, IntPtr.Zero, 1);
            }
            else
            {
                ExtractIconEx(filePath, index, IntPtr.Zero, out hIcon, 1);
            }

            return hIcon != IntPtr.Zero ? Icon.FromHandle(hIcon) : null;
        }

        public static Icon[] Extract(string filePath, int index)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            IntPtr hIconL;
            IntPtr hIconS;
            ExtractIconEx(filePath, index, out hIconL, out hIconS, 2);
            List<Icon> list = new List<Icon>();
            if (hIconL != IntPtr.Zero)
            {
                list.Add(Icon.FromHandle(hIconL));
            }
            if (hIconS != IntPtr.Zero)
            {
                list.Add(Icon.FromHandle(hIconS));
            }
            return list.ToArray();
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, int nIcons);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, out IntPtr phiconLarge, IntPtr phiconSmall, int nIcons);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr phiconLarge, out IntPtr phiconSmall, int nIcons);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool DestroyIcon(IntPtr hIcon);
    }
}
