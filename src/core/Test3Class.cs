using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Mitheti.Core
{
    public class Test3Class
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static string Test()
        {
            StringBuilder result = new StringBuilder();
            Process process = GetActiveProcess();

            if (process == null)
            {
                return "no process specified \n";
            }

            result.AppendLine($"title: {process.MainWindowTitle};");
            result.AppendLine($"name: {process.ProcessName};");
            result.AppendLine($"uptime: {process.UserProcessorTime};");
            result.AppendLine($"total: {process.TotalProcessorTime};");
            result.AppendLine($"startTime: {process.StartTime};\n");
            result.AppendLine($"session: {process.SessionId};");
            result.AppendLine($"isResponding: {process.Responding};");
            result.AppendLine($"id: {process.Id};");
            result.AppendLine($"isExited: {process.HasExited};");
            result.AppendLine($"basePriority: {process.BasePriority};");
            result.AppendLine($"machineName: {process.MachineName};");
            result.AppendLine($"---- end -----");

            return result.ToString();
        }

        private static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            return hwnd == null ? GetProcessByHandle(hwnd) : null;
        }

        private static Process GetProcessByHandle(IntPtr hwnd)
        {
            try
            {
                uint processId;
                GetWindowThreadProcessId(hwnd, out processId);
                return Process.GetProcessById((int)processId);
            }
            catch
            {
                return null;
            }
        }
    }
}
