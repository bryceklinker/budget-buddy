using System;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Buddy.Infrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : RegistrationAttribute
    {
        public Type InterfaceType { get; }

        public SingletonAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }

        public override void Register(Type implementationType, IServiceCollection services)
        {
            services.AddSingleton(InterfaceType, implementationType);
        }
    }
}
