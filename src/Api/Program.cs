using System.Diagnostics;
using System.IO;
using BudgetBuddy.Api.Bootstrap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace BudgetBuddy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .CaptureStartupErrors(true)
                .UseEnvironment("Development")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            if (Debugger.IsAttached)
                host.Run();
            else
                host.RunAsService();
        }
    }
}
