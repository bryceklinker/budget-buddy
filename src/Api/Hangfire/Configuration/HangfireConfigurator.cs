using System.IO;
using BudgetBuddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Api.Hangfire.Configuration
{
    public class HangfireConfigurator : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Hangfire", "Configuration", "hangfire.json"));
        }
    }
}