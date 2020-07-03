using System;

namespace Mitheti.Wpf
{
    public interface IServiceLauncher : IDisposable
    {
        void Start();
        void Stop();
    }
}
