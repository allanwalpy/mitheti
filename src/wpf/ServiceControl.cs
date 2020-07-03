using System;
using Microsoft.Extensions.Hosting;

using WebProgram = Mitheti.Web.Program;
using CoreProgram = Mitheti.Core.Program;

namespace Mitheti.Wpf
{
    // TODO:FIXME:;
    public class ServiceControll : IDisposable
    {
        private IHost _webService;
        private IHost _coreService;

        public ServiceControll()
        {
            //? NB! web app runs on Build() already;
            _coreService = null;
            _webService = WebProgram.CreateHostBuilder(new string[] {}).Build();

            _webService.RunAsync();
        }

        public void Start()
        {
            if (_coreService != null)
            {
                return;
            }

            _coreService = CoreProgram.CreateHostBuilder(new string[] {}).Build();
            _coreService.RunAsync();
        }

        public async void Stop()
        {
            await _coreService.StopAsync();
            _coreService = null;
        }

        public async void Dispose()
        {
            await _webService.StopAsync();
            _webService = null;

            await _coreService.StopAsync();
            _coreService = null;
        }
    }
}
