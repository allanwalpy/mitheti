using System.Diagnostics;

namespace Mitheti.Core.Watcher
{
    public class ProcessInfo
    {
        public const int NullDuration = -1;

        public string Name { get; }
        public int Duration { get; private set; }

        public ProcessInfo(Process info)
        {
            this.Name = info.ProcessName;
            this.Duration = NullDuration;
        }

        public void SetDuretion()
        {
            if (this.Duration > NullDuration)
            {
                return;
            }

            this.Duration = NullDuration;
        }
    }

}
