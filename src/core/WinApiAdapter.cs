using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Mitheti.Core
{
    public static class WinApiAdapter
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr windowHandle, out uint processId);

        public static ProcessInfo GetFocusedWindowInfo()
        {
            IntPtr windowHandle = GetForegroundWindow();
            Process process = (windowHandle == null) ? null : GetProcessByHandle(windowHandle);
            return new ProcessInfo(process);
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
