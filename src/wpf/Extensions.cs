using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mitheti.Wpf
{
    public static class Extensions
    {
        public static T GetService<T>(this IHost host) => host.Services.GetService<T>();
    }
}