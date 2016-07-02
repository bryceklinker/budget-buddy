using System.IO;
using System.Linq;
using System.Reflection;
using Budget.Buddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Xunit;

namespace Budget.Buddy.Infrastructure.Test.Configuration
{
    public class ConfiguratorLoaderTest
    {
        private readonly ConfiguratorLoader _configuratorLoader;

        public ConfiguratorLoaderTest()
        {
            var assembly = typeof(ConfiguratorLoaderTest).GetTypeInfo().Assembly;
            _configuratorLoader = new ConfiguratorLoader(assembly);
        }

        [Fact]
        public void Configure_ShouldConfigureAllConfiguratorsUsingBuilder()
        {
            var builder = new ConfigurationBuilder();

            _configuratorLoader.Configure(builder);
            Assert.Equal("fakeconfig.json", ((JsonConfigurationSource)builder.Sources.First()).Path);
        }
    }

    public class ConfiguratorFake : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "fakeconfig.json"));
        }
    }
}
