using System.IO;
using BudgetBuddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Api.Budgets.Shared.Configuration
{
    public class BudgetsConfigurator : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder
                .AddJsonFile(Path.Combine("Budgets", "Shared", "Configuration", "budgets.json"));
        }
    }
}