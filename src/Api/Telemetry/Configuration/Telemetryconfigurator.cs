using System.IO;
using BudgetBuddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Api.Telemetry.Configuration
{
    public class Telemetryconfigurator : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Telemetry", "Configuration", "telemetry.json"));
        }
    }
}