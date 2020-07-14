using System;

namespace Mitheti.Core.Services
{
    public class WatcherStatusEventArgs : EventArgs
    {
        public bool IsLaunched { get; }

        public WatcherStatusEventArgs(bool isLaunched)
        {
            this.IsLaunched = isLaunched;
        }
    }
}