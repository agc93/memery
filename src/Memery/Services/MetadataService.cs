using System;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace Memery.Services
{
    public class MetadataService {
        public static string APIName {get;} = "v1";
        
        public static string GetMetadataFilePath() {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
        public string Version {get;} = typeof(Startup).Assembly.GetName().Version.ToString();

        public string RuntimeVersion {get;} = typeof(RuntimeEnvironment).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

        public Info GetApiInfo() => new Info {
            Title = "Memery API",
            Description = "Because everyone knows throwing APIs at things solves so many problems",
            Version = this.Version,
            Contact = new Contact {
                Name = "Alistair Chapman"
            },
            License = new License { Name = "MIT" },
            TermsOfService = "Don't be a muppet"
        };
    }
}