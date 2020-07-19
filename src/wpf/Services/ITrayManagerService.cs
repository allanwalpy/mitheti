using System;
using Mitheti.Wpf.Views;

namespace Mitheti.Wpf.Services
{
    public interface ITrayManagerService : IDisposable
    {
        void Initialize(MainWindow window);
    }
}