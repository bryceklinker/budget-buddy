using System;
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
                .CaptureStartupErrors(true)
                .UseEnvironment("Development")
                .UseContentRoot(AppDirectory.Root);

            builder = Debugger.IsAttached ? builder.UseIISIntegration() : builder.UseUrls("http://bryce-8:8000");
            var host = builder
                .UseStartup<Startup>()
                .Build();

            try
            {
                if (Debugger.IsAttached)
                    host.Run();
                else
                    host.RunAsService();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
