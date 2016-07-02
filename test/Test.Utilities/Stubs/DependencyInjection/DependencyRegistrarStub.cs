using Budget.Buddy.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Buddy.Test.Utilities.Stubs.DependencyInjection
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
