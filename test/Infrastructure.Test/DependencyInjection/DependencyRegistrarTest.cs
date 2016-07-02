using System;
using System.Linq;
using System.Reflection;
using Budget.Buddy.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Budget.Buddy.Infrastructure.Test.DependencyInjection
{
    public class DependencyRegistrarTest
    {
        private readonly Assembly _assembly;
        private readonly IServiceCollection _services;
        private readonly DependencyRegistrar _dependencyRegistrar;

        public DependencyRegistrarTest()
        {
            _assembly = typeof(DependencyRegistrarTest).GetTypeInfo().Assembly;
            _services = new ServiceCollection();
            _dependencyRegistrar = new DependencyRegistrar(_assembly);
        }

        [Fact]
        public void Register_ShouldRegisterSingletons()
        {
            _dependencyRegistrar.Register(_services);
            AssertHasService(typeof(ISingletonDependency), typeof(SingletonDependency), ServiceLifetime.Singleton);
        }

        [Fact]
        public void Register_ShouldRegisterTrainsients()
        {
            _dependencyRegistrar.Register(_services);
            AssertHasService(typeof(ITransientDependency), typeof(TransientDependency), ServiceLifetime.Transient);
        }

        private void AssertHasService(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime)
        {
            Assert.True(_services.Any(s => s.ServiceType == serviceType 
            && s.ImplementationType == implementationType
            && s.Lifetime == serviceLifetime));
        }
    }

    public interface ITransientDependency
    {
        
    }

    [Transient(typeof(ITransientDependency))]
    public class TransientDependency : ITransientDependency
    {
        
    }


    public interface ISingletonDependency
    {
        
    }

    [Singleton(typeof(ISingletonDependency))]
    public class SingletonDependency : ISingletonDependency
    {
        
    }
}
