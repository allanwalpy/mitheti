using System;

namespace Mitheti.Wpf.Services
{
    public interface ITrayManagerService : IDisposable
    {
        event EventHandler WindowShowing;

        event EventHandler WindowExiting;
    }
}