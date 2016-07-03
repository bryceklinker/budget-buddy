using System.Linq;
using BudgetBuddy.Api.Budgets.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Configuration
{
    public class BudgetConfiguratorTest
    {
        [Fact]
        public void Configure_ShouldAddBudgetsConfig()
        {
            var builder = new ConfigurationBuilder();

            new BudgetsConfigurator().Configure(builder);
            Assert.Equal("budgets.json", ((JsonConfigurationSource) builder.Sources.First()).Path);
        }
    }
}
