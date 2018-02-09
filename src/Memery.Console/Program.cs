using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.CommandLine;
using Spectre.CommandLine.Extensions.DependencyInjection;
using static System.Console;

namespace Memery.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            var services = BuildServices().AddConfiguration();
            var app = new CommandApp(new DependencyInjectionRegistrar(services));
            app.Configure(config => {
                config.SetApplicationName("Memery CLI");
                config.AddCommand<Commands.UploadCommand>("upload");
                config.AddCommand<Commands.AddRemoteImageCommand>("add-image");
                config.AddCommand<Commands.ListCommand>("list");
                config.AddCommand<Commands.InfoCommand>("info");
            });
            return app.Run(args.RemoveHelp());
        }

        private static IServiceCollection BuildServices() {
            var services = new ServiceCollection();
            // services.Scan(scan => {
            //     scan.FromAssemblyOf<Program>()
            //         .AddClasses(classes => classes.AssignableTo(typeof(Command<>)))
            //         .AsSelf()
            //         .WithTransientLifetime();
            // });
            services.AddSingleton<System.Net.Http.HttpMessageHandler>(MemeryClient.GetHandler);
            services.AddTransient<MemeryClient>();
            return services;
        }
    }
}
