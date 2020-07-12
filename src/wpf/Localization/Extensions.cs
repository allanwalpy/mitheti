using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Mitheti.Wpf.Localization
{
    public static class Extensions
    {
        //юзается один раз, еще и экстеншен, нужно заинлайнить
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            return services;
        }
    }
}
