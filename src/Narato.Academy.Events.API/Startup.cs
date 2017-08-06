using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Narato.Correlations;
using Narato.ExecutionTimingMiddleware;
using Narato.ResponseMiddleware;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Narato.Academy.Events.DataProvider.Mappers;
using Swashbuckle.Swagger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Narato.Academy.Events.API.Mappers;
using Narato.Academy.Events.DataProvider.Contexts;
using Microsoft.EntityFrameworkCore;
using Narato.Academy.Events.DataProvider.DataProviders;
using Narato.Academy.Events.Domain.Contracts.DataProviders;
using Narato.Academy.Events.Domain.Managers;
using Narato.Academy.Events.Domain.Managers.Interfaces;

namespace Narato.Academy.Events.API
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddJsonFile("config.json.local", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("nlog.config");

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DataProviderAutoMapperProfileConfiguration());
                cfg.AddProfile(new DTOAutoMapperProfileConfiguration());
            });
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(
            //Add this filter globally so every request runs this filter to recored execution time
                config =>
                {
                    config.AddResponseFilters();
                })
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ContractResolver =
                     new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }
            );

            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<DataContext>(options => {
                options.UseNpgsql(Configuration["DatabaseConfiguration:ConnectionString"]);
            });

            services.AddTransient<IEventDataProvider, EventDataProvider>();
            services.AddTransient<IEventManager, EventManager>();

            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Contact = new Contact { Name = "Narato NV" },
                    Description = "Narato Libraries Narato.Academy.Events.API API",
                    Version = "v1",
                    Title = "Narato.Academy.Events.API"
                });
                //options.OperationFilter<ProducesConsumesFilter>();

                var xmlPaths = GetXmlCommentsPaths();
                foreach (var entry in xmlPaths)
                {
                    try
                    {
                        options.IncludeXmlComments(entry);
                    }
                    catch (Exception e)
                    {
                    }
                }
            });

            services.AddCorrelations();
            services.AddResponseMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // auto migrations
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseExceptionHandler();
            app.UseCorrelations();
            app.UseExecutionTiming();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

            app.UseMvc();
        }

        private List<string> GetXmlCommentsPaths()
        {
            var app = PlatformServices.Default.Application;
            var files = new List<string>()
                        {
                            "Narato.Academy.Events.API.xml"
                        };

            List<string> paths = new List<string>();
            foreach (var file in files)
            {
                paths.Add(Path.Combine(app.ApplicationBasePath, file));
            }

            return paths;
        }
    }
}
