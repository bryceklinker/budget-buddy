using System.Threading.Tasks;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.Budgets.Exists
{
    public interface IBudgetExistsQuery
    {
        Task<bool> Execute(int month, int year);
    }

    [Transient(typeof(IBudgetExistsQuery))]
    public class BudgetExistsQuery : IBudgetExistsQuery
    {
        private readonly IRepository<Budget> _budgetRepository;

        public BudgetExistsQuery(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<bool> Execute(int month, int year)
        {
            var budgets = await _budgetRepository.GetAll();
            return budgets
                .Where(b => b.StartDate.Month == month)
                .Any(b => b.StartDate.Year == year);
        }
    }
}