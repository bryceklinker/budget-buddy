using Microsoft.Extensions.Configuration;

namespace BudgetBuddy.Infrastructure.Configuration
{
    public interface IConfigurator
    {
        void Configure(IConfigurationBuilder builder);
    }
}
