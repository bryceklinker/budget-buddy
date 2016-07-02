using Microsoft.Extensions.Configuration;

namespace Budget.Buddy.Infrastructure.Configuration
{
    public interface IConfigurator
    {
        void Configure(IConfigurationBuilder builder);
    }
}
