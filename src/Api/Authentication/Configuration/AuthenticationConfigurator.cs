using System.IO;
using Budget.Buddy.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace Budget.Buddy.Api.Authentication.Configuration
{
    public class AuthenticationConfigurator : IConfigurator
    {
        public void Configure(IConfigurationBuilder builder)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Authentication", "Configuration", "authsettings.json");
            builder.AddJsonFile(path);
        }
    }
}
