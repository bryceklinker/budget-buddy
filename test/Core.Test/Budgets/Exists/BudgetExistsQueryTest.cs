using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Exists;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;

namespace BudgetBuddy.Core.Test.Budgets.Exists
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
            _budgetContext.Add(new Budget {Month = 4, Year = 2013});
            await _budgetContext.SaveChangesAsync();

            var exists = await _budgetExistsQuery.Execute(4, 2013);
            Assert.True(exists);
        }

        [Fact]
        public async Task Execute_ShouldBeFalse()
        {
            _budgetContext.Add(new Budget { Month = 4, Year = 2013 });
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
