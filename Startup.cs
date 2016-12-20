using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zippy.Controllers.Filters;
using Zippy.Data.Contract;
using Zippy.Data.Providers;
using Zippy.Services.Contract;
using Zippy.Services;
using Zippy.Utils;

namespace Zippy
{
    public class Startup
    {
        private const bool UnderSimulation = false;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(env.ContentRootPath)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                     .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var loggerFactory = Helpers.CreateLoggerFactory();
            AddMvc(services, loggerFactory);

            services.AddSingleton<ILoggerFactory>(sp => loggerFactory);
            //services.AddSingleton<IGeocodingServiceProvider, SimulatedGeoCodingService>();

            services.AddSingleton<IZCoreService, ZCoreService>();
            services.AddSingleton<IZCache, ZCache>();
            services.AddSingleton<IZRepository, ZRepository>();
            services.AddSingleton<IZDbDriver, DumbDbDriver>();

            services.AddSingleton<IGeocodingServiceProvider>((sp) =>
            {
                if (UnderSimulation)
                {
                    return new SimulatedGeoCodingService(loggerFactory);
                }

                return new GoogleGeoCodingService(loggerFactory);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Zippy}/{action=Index}/{id?}");
            });
        }

        private void AddMvc(IServiceCollection services, ILoggerFactory logger)
        {
            var builder = services.AddMvc();
            builder.AddMvcOptions(o => { o.Filters.Add(new ZExceptionFilter(logger)); });
        }
    }
}
