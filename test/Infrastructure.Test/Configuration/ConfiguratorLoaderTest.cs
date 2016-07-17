using System.Reflection;
using BudgetBuddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;


namespace BudgetBuddy.Infrastructure.Test.Configuration
{
    
    public class ConfiguratorLoaderTest
    {
        private readonly ConfiguratorLoader _configuratorLoader;

        public ConfiguratorLoaderTest()
        {
            var assembly = typeof(ConfiguratorFake).GetTypeInfo().Assembly;
            _configuratorLoader = new ConfiguratorLoader(assembly);
        }

        [Fact]
        public void Configure_ShouldConfigureAllConfiguratorsUsingBuilder()
        {
            var builder = new ConfigurationBuilder();

            _configuratorLoader.Configure(builder);
            Assert.True(ConfiguratorFake.IsConfigured);
        }
    }

    public class ConfiguratorFake : IConfigurator
    {
        public static bool IsConfigured { get; private set; }

        public ConfiguratorFake()
        {
            IsConfigured = false;
        }

        public void Configure(IConfigurationBuilder builder)
        {
            IsConfigured = true;
        }
    }
}
