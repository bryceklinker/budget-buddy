using System.Reflection;
using Budget.Buddy.Infrastructure.Configuration;
using Budget.Buddy.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Budget.Buddy.Api.Bootstrap
{
    public class Startup
    {
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
                .AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddMvc();
            _dependencyRegistrar.Register(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(c =>
            {
                c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            })
                .UseIdentity()
                .UseGoogleAuthentication(new GoogleOptions
                {
                    ClientId = Configuration["GoogleAuth:ClientId"],
                    ClientSecret = Configuration["GoogleAuth:ClientSecret"]
                })
                .UseMvc();
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
