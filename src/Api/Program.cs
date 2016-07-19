using System.Diagnostics;
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
                .UseContentRoot(AppDirectory.Root)
                .UseUrls("http://bryce-8:8000")
                .UseStartup<Startup>()
                .Build();

            if (Debugger.IsAttached)
                host.Run();
            else
                host.RunAsService();
        }
    }
}
