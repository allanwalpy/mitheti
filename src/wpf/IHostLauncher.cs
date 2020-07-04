using System;

namespace Mitheti.Wpf
{
    public interface IHostLauncher : IDisposable
    {
        bool IsLaunched { get; }
        void Start();
        void Stop();
    }
}
