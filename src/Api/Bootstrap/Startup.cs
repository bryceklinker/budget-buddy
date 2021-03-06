﻿using System.IO;
using System.Reflection;
using BudgetBuddy.Api.Telemetry;
using BudgetBuddy.Infrastructure.Configuration;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BudgetBuddy.Api.Bootstrap
{
    public class Startup
    {
        private const string CorsPolicyName = "Default";
        private static readonly Assembly StartupAssembly = typeof(Startup).GetTypeInfo().Assembly;
        private readonly IDependencyRegistrar _dependencyRegistrar;
        private readonly IConfiguratorLoader _configuratorLoader;
        private IConfiguration _configuration;

        public IConfiguration Configuration => _configuration ?? (_configuration = CreateConfig());

        public Startup()
            : this(new DependencyRegistrar(StartupAssembly), new ConfiguratorLoader(StartupAssembly))
        {

        }

        public Startup(IDependencyRegistrar dependencyRegistrar, IConfiguratorLoader configuratorLoader )
        {
            _dependencyRegistrar = dependencyRegistrar;
            _configuratorLoader = configuratorLoader;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IConfiguration), Configuration)
                .AddCors(o => o.AddPolicy(CorsPolicyName, b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()))
                .AddMvc();
            _dependencyRegistrar.Register(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage()
                .UseMiddleware<TelemetryMiddleware>()
                .UseCors(CorsPolicyName)
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseMvc();
        }

        private IConfiguration CreateConfig()
        {
            File.WriteAllText("budget-buddy.log.txt", AppDirectory.Root);
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDirectory.Root);
            _configuratorLoader.Configure(builder);
            return builder
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
