using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebProgram = Mitheti.Web.Program;
using CoreProgram = Mitheti.Core.Program;

namespace Mitheti.Wpf
{
    public class ServiceControll : IDisposable
    {
        private IHost _coreService;
        private IHost _webService;

        public ServiceControll()
        {
            _coreService = null;

            _webService =  WebProgram.CreateHostBuilder(new string[] {}).Build();
            Task.Run(_webService.Run);
        }

        public void Start()
        {
            if (_coreService != null)
            {
                return;
            }

            _coreService = CoreProgram.CreateHostBuilder(new string[] {}).Build();
            Task.Run(_coreService.Run);
        }

        public async void Stop()
        {
            await _coreService.StopAsync();
            _coreService = null;
        }

        public async void Dispose()
        {
            await Task.WhenAll(
                _coreService.StopAsync(),
                _webService.StopAsync());

            _coreService = null;
            _webService = null;
        }
    }
}
