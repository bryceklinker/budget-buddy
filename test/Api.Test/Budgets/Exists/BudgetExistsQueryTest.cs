using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Exists;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Exists
{
    public class BudgetExistsQueryTest
    {
        private readonly BudgetContext _budgetContext;
        private readonly BudgetExistsQuery _budgetExistsQuery;

        public BudgetExistsQueryTest()
        {
            _budgetContext = DbContextFactory.Create<BudgetContext>();
            _budgetExistsQuery = new BudgetExistsQuery(_budgetContext);
        }

        [Fact]
        public async Task Execute_ShouldBeTrue()
        {
            _budgetContext.Add(new Budget {StartDate = new DateTime(2013, 4, 1)});
            await _budgetContext.SaveChangesAsync();

            var exists = await _budgetExistsQuery.Execute(4, 2013);
            Assert.True(exists);
        }

        [Fact]
        public async Task Execute_ShouldBeFalse()
        {
            _budgetContext.Add(new Budget {StartDate = new DateTime(2013, 4, 1)});
            await _budgetContext.SaveChangesAsync();

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
