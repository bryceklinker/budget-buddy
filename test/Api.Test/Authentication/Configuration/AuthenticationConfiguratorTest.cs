using System.Linq;
using Budget.Buddy.Api.Authentication.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Xunit;

namespace Budget.Buddy.Api.Test.Authentication.Configuration
{
    public class AuthenticationConfiguratorTest
    {
        private readonly AuthenticationConfigurator _authenticationConfigurator;

        public AuthenticationConfiguratorTest()
        {
            _authenticationConfigurator = new AuthenticationConfigurator();
        }

        [Fact]
        public void Configure_ShouldAddAuthenticationConfig()
        {
            var builder = new ConfigurationBuilder();

            _authenticationConfigurator.Configure(builder);
            Assert.Equal("authsettings.json", ((JsonConfigurationSource) builder.Sources.First()).Path);
        }
    }
}
