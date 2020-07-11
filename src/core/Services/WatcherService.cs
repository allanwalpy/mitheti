using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mitheti.Core.Services
{
    public class WatcherService : IWatcherService
    {
        public const string DelayConfigKey = "service:delay";

        private readonly IFilterApp _filter;
        private readonly int _delay;

        public WatcherService(IConfiguration config, IFilterApp filter)
        {
            _filter = filter;

            //в конфиге этого хначения может не быть, тогда вернется default(int), те 0
            _delay = config.GetValue<int>(DelayConfigKey);
        }

        //rename to RunAsync
        //похорошему лучше таску наружу не выставлять а крутить внутри сервиса, локализовать её работу так сказать
        public async Task Run(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var processName = WinApiAdapter.GetFocusedWindowInfo()?.ProcessName;
                //странно что допускается добавление null
                _filter.Add(processName, _delay);

                await Task.Delay(_delay, stoppingToken).ThrowNoExceptionOnCancelled();
            }
        }
    }
}
