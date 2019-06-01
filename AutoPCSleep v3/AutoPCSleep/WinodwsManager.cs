using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;


namespace AutoPCSleep
{

        public class WindowsManager
        {                           
            delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll", SetLastError = true)]
            static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool IsWindowVisible(IntPtr hWnd);

            public static string GetWindowText(IntPtr hWnd)
            {
                int len = GetWindowTextLength(hWnd) + 1;
                StringBuilder sb = new StringBuilder(len);
                len = GetWindowText(hWnd, sb, len);
                return sb.ToString(0, len);
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
            public static void KillWinByT(string NameOfWindow)
            {
                int pid;
                string search = NameOfWindow;
                string currWindow = null;
                if (!EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
                {
                    currWindow = GetWindowText(hWnd);
                    if (!string.IsNullOrEmpty(currWindow) && currWindow.Contains(search))
                    {
                        GetWindowThreadProcessId(hWnd, out pid);
                        Process.GetProcessById(pid).Kill();
                    }
                    return true;
                }, IntPtr.Zero))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }




        }





    

}
