using System.IO;
using BudgetBuddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Core.Budgets.Configuration
{
    public class BudgetsConfigurator : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Budgets", "Configuration", "budgets.json"));
        }
    }
}