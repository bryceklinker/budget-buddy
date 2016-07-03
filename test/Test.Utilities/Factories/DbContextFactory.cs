using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetBuddy.Test.Utilities.Factories
{
    public static class DbContextFactory
    {
        public static T Create<T>() where T : DbContext
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            return (T)Activator.CreateInstance(typeof(T), options);
        }
    }
}
