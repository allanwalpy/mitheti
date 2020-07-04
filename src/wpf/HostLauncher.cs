using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebProgram = Mitheti.Web.Program;
using CoreProgram = Mitheti.Core.Program;

namespace Mitheti.Wpf
{
    public class HostLauncher : IHostLauncher
    {
        public const int DelayOnWebHostStart = 250;
        private IHost _coreHost;
        private IHost _webHost;

        public HostLauncher()
        {
            _coreHost = null;

            _webHost = WebProgram.CreateHostBuilder(new string[] { }).Build();
            Task.Run(async () =>
            {
                //? delay web host start, so it won't affect wpf app start;
                await Task.Delay(DelayOnWebHostStart);
                await _webHost.StartAsync();
            } );
        }

        public void Start()
        {
            if (_coreHost != null)
            {
                return;
            }
            _coreHost = CoreProgram.CreateHostBuilder(new string[] { }).Build();
            _coreHost.StartAsync();
        }

        public async void Stop()
        {
            if (_coreHost == null)
            {
                return;
            }

            await _coreHost.StopAsync();
            _coreHost.Dispose();
            _coreHost = null;
        }

        //TODO:FIXME: sort out deadlocks and crashes on application exit;
        public void Dispose()
        {
            this.Stop();

            //? FIXME: web service won't stop, resulting in deadlock on `Task.WhenAll`;
            _webHost.Dispose();
            _webHost = null;
        }
    }
}
