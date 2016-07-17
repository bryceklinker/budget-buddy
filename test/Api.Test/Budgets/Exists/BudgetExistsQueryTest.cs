using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Exists;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Exists
{
    
    public class BudgetExistsQueryTest
    {
        private readonly InMemoryRepository<Budget> _budgetContext;
        private readonly BudgetExistsQuery _budgetExistsQuery;

        public BudgetExistsQueryTest()
        {
            _budgetContext = new InMemoryRepository<Budget>();
            _budgetExistsQuery = new BudgetExistsQuery(_budgetContext);
        }

        [Fact]
        public async Task Execute_ShouldBeTrue()
        {
            await _budgetContext.Insert(new Budget {StartDate = new DateTime(2013, 4, 1)});

            var exists = await _budgetExistsQuery.Execute(4, 2013);
            Assert.True(exists);
        }

        [Fact]
        public async Task Execute_ShouldBeFalse()
        {
            await _budgetContext.Insert(new Budget {StartDate = new DateTime(2013, 4, 1)});

            var exists = await _budgetExistsQuery.Execute(5, 2013);
            Assert.False(exists);
        }

        [Fact]
        public void BudgetExsists_ShouldBeTransient()
        {
            var transient = _budgetExistsQuery.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IBudgetExistsQuery), transient.InterfaceType);
        }
    }
}
