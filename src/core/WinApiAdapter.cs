using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mitheti.Core
{
    public static class WinApiAdapter
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr windowHandle, out uint processId);

        public static Process GetFocusedWindowInfo()
        {
            var windowHandle = GetForegroundWindow();
            return GetProcessByHandle(windowHandle);
        }

        private static Process GetProcessByHandle(IntPtr windowHandle)
        {
            try
            {
                GetWindowThreadProcessId(windowHandle, out var processId);
                return Process.GetProcessById((int) processId);
            }
            catch
            {
                return null;
            }
        }
    }
}