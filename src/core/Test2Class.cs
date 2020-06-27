using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Mitheti.Core
{
    public static class Test2Class
    {

        [DllImport("user32.dll")]
        private extern static IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindow()
        {
            const int nChars = 256;
            IntPtr handle;
            StringBuilder buffer = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            if (GetWindowText(handle, buffer, nChars) == 0)
            {
                return null;
            }
            return buffer.ToString();
        }
    }
}
