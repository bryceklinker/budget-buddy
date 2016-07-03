using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Test.Utilities.Stubs.DependencyInjection
{
    public class DependencyRegistrarStub : IDependencyRegistrar
    {
        public IServiceCollection Services { get; private set; }

        public void Register(IServiceCollection services)
        {
            Services = services;
        }
    }
}
