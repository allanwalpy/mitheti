using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Mitheti.Core
{
    public class GetActiveWindow
    {
        private delegate bool EnumWindowsProc(IntPtr handleWindow, int hadles); //ArrayList handles);

        [DllImport("user32")]
        private static extern int GetWindowLongA(IntPtr hWnd, int index);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private const int GWL_STYLE = -16;

        private const ulong WS_VISIBLE = 0x10000000L;
        private const ulong WS_BORDER = 0x00800000L;
        private const ulong TARGET_WINDOW = WS_BORDER | WS_VISIBLE;

        internal class Window
        {
            public string Title;
            public IntPtr Handle;

            public override string ToString()
                => this.Title;
        }

        private List<Window> _windows;

        private bool Callback(IntPtr hwnd, int lParam)
        {
            bool isContinue = true; //(GetWindowLongA(hwnd, GWL_STYLE) & TARGET_WINDOW) == TARGET_WINDOW;
            if (!isContinue)
            {
                return true;
            }

            StringBuilder sb = new StringBuilder(100);
            GetWindowText(hwnd, sb, sb.Capacity);

            Window t = new Window();
            t.Handle = hwnd;
            t.Title = sb.ToString();
            this._windows.Add(t);

            return true;
        }

        private void GetWindows()
        {
            this._windows = new List<Window>();
            EnumWindows(Callback, 0);
        }

        public void Test()
        {
            IntPtr selectedWindow = GetForegroundWindow();
            this.GetWindows();

            for (int i = 0; i < this._windows.Count; i++)
            {
                Console.WriteLine($"#{i} is {_windows[i].Handle} and {_windows[i].Title}; trigger is {selectedWindow == _windows[i].Handle};");
            }
        }

    }
}
