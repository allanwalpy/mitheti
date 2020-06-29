using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mitheti.Core.Watcher
{
    public static class WinApiAdapter
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr windowHandle, out uint processId);

        public static Process GetFocusedWindowInfo()
        {
            IntPtr windowHandle = GetForegroundWindow();
            return (windowHandle == null) ? null : GetProcessByHandle(windowHandle);
        }

        private static Process GetProcessByHandle(IntPtr windowHandle)
        {
            try
            {
                uint processId;
                GetWindowThreadProcessId(windowHandle, out processId);
                return Process.GetProcessById((int)processId);
            }
            catch
            {
                return null;
            }
        }
    }
}
