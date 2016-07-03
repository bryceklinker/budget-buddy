using System;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Infrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class RegistrationAttribute : Attribute
    {
        public abstract void Register(Type type, IServiceCollection services);
    }
}
