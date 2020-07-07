using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebProgram = Mitheti.Web.Program;
using CoreProgram = Mitheti.Core.Program;

namespace Mitheti.Wpf
{
    public class HostLauncher : IDisposable
    {
        public const int DelayOnWebHostStart = 500;

        private IHost _coreHost;
        private IHost _webHost;

        public event EventHandler OnStatusChange;

        public HostLauncher()
        {
            _coreHost = null;

            _webHost = WebProgram.CreateHostBuilder(new string[0]).Build();
            Task.Run(this.StartWebHostWithDelay);
        }

        private async Task StartWebHostWithDelay()
        {
            await Task.Delay(DelayOnWebHostStart);
            await _webHost.StartAsync();
        }

        public async Task StartAsync()
        {
            if (_coreHost != null)
            {
                return;
            }

            _coreHost = CoreProgram.CreateHostBuilder(new string[0]).Build();
            await _coreHost.StartAsync();

            this.TriggerStatusChange(isLaunched: true);
        }

        public async Task StopAsync()
        {
            if (_coreHost == null)
            {
                return;
            }

            await _coreHost.StopAsync();
            _coreHost.Dispose();
            _coreHost = null;

            this.TriggerStatusChange(isLaunched: false);
        }

        private void TriggerStatusChange(bool isLaunched)
        {
            this.OnStatusChange?.Invoke(null, new StatusChangeEventArgs() { IsLaunched = isLaunched });
        }

        //TODO:FIXME: sort out deadlocks and crashes on application exit;
        public void Dispose()
        {
            this.StopAsync().Wait();

            //? FIXME: web service won't stop, resulting in deadlock on `Task.WhenAll`;
            _webHost.Dispose();
            _webHost = null;
        }
    }
}
