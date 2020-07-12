using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mitheti.Core.Services;

namespace Mitheti.Worker
{
    public static class Program
    {
        //что за вокрер? воркер чего? по названию проекта ничего не понятно, вообще навазвания всех проектов странные еще и с маленькой буквы
        //весь этот проект можно убрать(его содержимое перенести в другой проект) в нем толком нет нагрузки
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCoreConfiguration()
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCoreServices();
                    services.AddHostedService<Worker>();
                });
    }
}
