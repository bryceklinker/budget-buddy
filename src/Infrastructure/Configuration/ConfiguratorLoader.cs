using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Budget.Buddy.Infrastructure.Configuration
{
    public interface IConfiguratorLoader
    {
        void Configure(IConfigurationBuilder builder);
    }

    public class ConfiguratorLoader : IConfiguratorLoader
    {
        private readonly Assembly _assembly;

        public ConfiguratorLoader(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void Configure(IConfigurationBuilder builder)
        {
            _assembly.GetTypes()
                .Where(IsConfigurator)
                .Select(Activator.CreateInstance)
                .Cast<IConfigurator>()
                .ForEach(c => c.Configure(builder));
        }

        private bool IsConfigurator(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IConfigurator));
        }
    }
}
