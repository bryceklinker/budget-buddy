using System.Diagnostics;
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
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>();

            if (IsDebugging())
            {
                builder = builder.UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration();
            }
            else
            {
                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var directory = Path.GetDirectoryName(exePath);
                builder = builder.UseContentRoot(directory)
                    .UseUrls("http://+:8000");
            }

            var host = builder.Build();

            var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
            if (hostingEnvironment.IsProduction())
                host.RunAsService();
            else
                host.Run();
        }

        private static bool IsDebugging()
        {
            return Debugger.IsAttached;
        }
    }
}
