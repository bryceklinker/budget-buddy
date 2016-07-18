using System.IO;
using BudgetBuddy.Api.Bootstrap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
            if (hostingEnvironment.IsProduction())
                host.RunAsService();
            else
                host.Run();
        }
    }
}
