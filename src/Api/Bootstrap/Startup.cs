﻿using System.Reflection;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Telemetry;
using BudgetBuddy.Api.Telemetry.Model;
using BudgetBuddy.Infrastructure.Configuration;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Hangfire;
using Hangfire.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

        public Startup(IDependencyRegistrar dependencyRegistrar, IConfiguratorLoader configuratorLoader)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _configuratorLoader = configuratorLoader;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IConfiguration), Configuration)
                .AddCors(o => o.AddPolicy(CorsPolicyName, b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()))
                .AddHangfire(g => g.UseDefaultActivator().UseSqlServerStorage(Configuration["Hangfire:ConnectionString"]))
                .AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BudgetContext>(o => o.UseSqlServer(Configuration["Budgets:ConnectionString"]))
                .AddDbContext<TelemetryContext>(o => o.UseSqlServer(Configuration["Telemetry:ConnectionString"]))
                .AddMvc();
            _dependencyRegistrar.Register(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<TelemetryMiddleware>()
                .UseCors(CorsPolicyName)
                .UseFileServer("/client")
                .UseDefaultFiles("/client")
                .UseHangfireDashboard()
                .UseHangfireServer(new BackgroundJobServerOptions
                {
                    Activator = new AspNetCoreJobActivator(app.ApplicationServices.GetService<IServiceScopeFactory>())
                })
                .UseMvc();

            var recurringManager = new RecurringJobManager();
            recurringManager.AddOrUpdate(CopyBudgetCommand.JobId, CopyBudgetCommand.Job, Cron.Monthly());
            BackgroundJob.Enqueue<ICopyBudgetCommand>(command => command.Execute());
        }

        private IConfiguration CreateConfig()
        {
            var builder = new ConfigurationBuilder();
            _configuratorLoader.Configure(builder);
            return builder
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
