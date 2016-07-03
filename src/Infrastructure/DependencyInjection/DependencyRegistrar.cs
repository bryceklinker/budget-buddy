using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Infrastructure.DependencyInjection
{
    public interface IDependencyRegistrar
    {
        void Register(IServiceCollection services);
    }

    public class DependencyRegistrar : IDependencyRegistrar
    {
        private readonly Assembly _assembly;

        public DependencyRegistrar(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void Register(IServiceCollection services)
        {
            _assembly.GetTypes()
                .Where(HasRegistration)
                .Select(t => new
                {
                    Type = t,
                    Registration = GetRegistration(t)
                })
                .ForEach(t => t.Registration.Register(t.Type, services));
        }

        private static RegistrationAttribute GetRegistration(Type type)
        {
            return type.GetTypeInfo()
                .GetCustomAttribute<RegistrationAttribute>();
        }

        private static bool HasRegistration(Type type)
        {
            return type.GetTypeInfo()
                .GetCustomAttributes<RegistrationAttribute>()
                .Any();
        }
    }
}
