using System;
using System.Diagnostics;
using System.Text;

namespace Mitheti.Core.Watcher
{
    public class ProcessInfo
    {
        public Category Category { get; }
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
            this.Category = Category.Null;
        }

        public ProcessInfo(Process info)
            : this(
                name        : info.ProcessName,
                id          : info.Id,
                windowTitle : info.MainWindowTitle,
                uptime      : info.TotalProcessorTime,
                startedAt   : info.StartTime)
        {
            this.Category = Category.Defined;
        }

        private ProcessInfo(string name, int id, string windowTitle, TimeSpan uptime, DateTime startedAt)
        {
            this.Name = name ?? name.ToLower();
            this.Id = id;
            this.WindowTitle = windowTitle;
            this.Uptime = uptime;
            this.StartedAt = startedAt;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"{nameof(Category)}={this.Category.ToString()};");
            if (this.Category == Category.Null)
            {
                return result.ToString();
            }

            result.Append($"{nameof(Name)        }={this.Name        };");
            result.Append($"{nameof(Id)          }={this.Id          };");
            result.Append($"{nameof(WindowTitle) }={this.WindowTitle };");
            result.Append($"{nameof(Uptime)      }={this.Uptime      };");
            result.Append($"{nameof(StartedAt)   }={this.StartedAt   };");

            return result.ToString();
        }
    }

}
