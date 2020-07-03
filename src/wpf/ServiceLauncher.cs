using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebProgram = Mitheti.Web.Program;
using CoreProgram = Mitheti.Core.Program;

namespace Mitheti.Wpf
{
    public class ServiceLauncher : IServiceLauncher
    {
        private IHost _coreService;
        private IHost _webService;

        public ServiceLauncher()
        {
            _coreService = null;
            _webService = null;
        }

        public void Start()
        {
            if (_coreService == null)
            {
                _coreService = CoreProgram.CreateHostBuilder(new string[] { }).Build();
                _coreService.StartAsync();
            }

            if (_webService == null)
            {
                _webService = WebProgram.CreateHostBuilder(new string[] { }).Build();
                _webService.StartAsync();
            }
        }

        public async void Stop()
        {
            if (_coreService == null)
            {
                return;
            }

            await _coreService.StopAsync();
            _coreService.Dispose();
            _coreService = null;
        }

        public async void Dispose()
        {
            if (_coreService != null)
            {
                this.Stop();
            }

            //? FIXME: web service won't stop, resulting in deadlock on `Task.WhenAll`;
            //await _webService.StopAsync();
            //_webService.Dispose();
            _webService = null;
        }
    }
}
