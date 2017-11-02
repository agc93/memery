using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memery.Console
{
    internal static class CoreExtensions
    {
        internal static IServiceCollection AddConfiguration(this IServiceCollection services) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddEnvironmentVariables("MEMERY_")
                .AddEnvironmentVariables("MEMERY")
                .AddJsonFile("./appsettings.json", optional: true);
            var config = builder.Build();
            services.AddSingleton<IConfiguration>(config);
            services.AddOptions().Configure<MemeryConsoleOptions>(config);
            return services;
        }

        internal static string[] RemoveHelp(this string[] args) {
            args = args.Where(a => a != "-h" && a != "--help").ToArray();
            return args;
        }
    }
}