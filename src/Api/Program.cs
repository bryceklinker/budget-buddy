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
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(BaseDirectory.GetBaseDirectory())
                .UseStartup<Startup>();

            builder = IsDebugging() 
                ? builder.UseIISIntegration() 
                : builder.UseUrls("http://+:8000");

            var host = builder.Build();

            if (!IsDebugging())
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
