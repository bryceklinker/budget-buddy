using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Core.Budgets.Exists
{
    public interface IBudgetExistsQuery
    {
        Task<bool> Execute(int month, int year);
    }

    [Transient(typeof(IBudgetExistsQuery))]
    public class BudgetExistsQuery : IBudgetExistsQuery
    {
        private readonly BudgetContext _budgetContext;

        public BudgetExistsQuery(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        public Task<bool> Execute(int month, int year)
        {
            return _budgetContext.Budgets
                .Where(b => b.Month == month)
                .AnyAsync(b => b.Year == year);
        }
    }
}