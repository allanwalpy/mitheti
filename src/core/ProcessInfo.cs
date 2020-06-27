using System;
using System.Diagnostics;

namespace Mitheti.Core
{

    public class ProcessInfo
    {
        public ProcessType Type { get; }
        public string Name { get; }
        public int Id { get;}
        public string WindowTitle { get; }
        public TimeSpan Uptime { get; }
        public DateTime StartedAt { get; }

        public ProcessInfo()
            : this (
                name        : null,
                id          : -1,
                windowTitle : null,
                uptime      : TimeSpan.MinValue,
                startedAt   : DateTime.Now)
        {
            this.Type = ProcessType.Null;
        }

        public ProcessInfo(Process info)
            : this(
                name        : info.ProcessName,
                id          : info.Id,
                windowTitle : info.MainWindowTitle,
                uptime      : info.TotalProcessorTime,
                startedAt   : info.StartTime)
        {
            this.Type = ProcessType.Defined;
        }

        private ProcessInfo(string name, int id, string windowTitle, TimeSpan uptime, DateTime startedAt)
        {
            this.Name = name ?? name.ToLower();
            this.Id = id;
            this.WindowTitle = windowTitle;
            this.Uptime = uptime;
            this.StartedAt = startedAt;
        }
    }

}
