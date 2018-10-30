using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Memery.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions().Configure<Configuration.MemeryOptions>(Configuration);
            services.AddMvc(opts =>
                    opts.Filters.Add<Infrastructure.IndexFileFilter>()
                )
                .AddXmlSerializerFormatters()
                .AddJsonOptions(o =>
                    o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                );
            // ServiceProvider serviceProvider = services.BuildServiceProvider();
            // IHostingEnvironment env = serviceProvider.GetService<IHostingEnvironment>();
            // if (env.IsProduction())
            // {
                services.AddSpaStaticFiles(configuration =>
                {
                    // configuration.RootPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ui", "dist");
                    configuration.RootPath = Path.Combine("ui", "dist");
                });
            // }
            services.AddMediatR();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(MetadataService.APIName, new MetadataService().GetApiInfo());
                c.IncludeXmlComments(MetadataService.GetMetadataFilePath());
            });
            services
                .AddSingleton<MetadataService, MetadataService>()
                .AddSingleton<IImageIndexService, ImageIndexService>()
                .AddSingleton<IFileUploadService, FileUploadService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseSwagger(c => {
                c.RouteTemplate = "/api/{documentName}";
            });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint($"/api/{MetadataService.APIName}", $"Memery API ({MetadataService.APIName})");
                c.RoutePrefix = "help";
                c.DocumentTitle = "Memery API";
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // app.MapWhen(c => !c.Request.Path.Value.EndsWith(".js"), config =>{
                app.UseMvc(routes =>
            {
                // routes.MapRoute(
                //     name: "default",
                //     template: "{controller}/{action?}/{id?}");

                // routes.MapSpaFallbackRoute(
                //     name: "spa-fallback",
                //     defaults: new { controller = "Home", action = "Index" });
            });
            // });
            app.UseSpa(spa =>
            {
                // spa.Options.
                spa.Options.SourcePath = "ui";
                if (env.IsDevelopment())
                {
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            // app
        }
    }
}
