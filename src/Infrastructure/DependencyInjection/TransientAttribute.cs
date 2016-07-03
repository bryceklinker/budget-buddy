using System;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Infrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientAttribute : RegistrationAttribute
    {
        public Type InterfaceType { get; }

        public TransientAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }

        public override void Register(Type implementationType, IServiceCollection services)
        {
            services.AddTransient(InterfaceType, implementationType);
        }
    }
}
