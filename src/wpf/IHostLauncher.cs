using System;

namespace Mitheti.Wpf
{
    public interface IHostLauncher : IDisposable
    {
        void Start();
        void Stop();
    }
}
