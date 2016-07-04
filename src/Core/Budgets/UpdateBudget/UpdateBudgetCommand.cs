using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Mappers;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Core.Budgets.UpdateBudget
{
    public interface IUpdateBudgetCommand
    {
        Task Execute(BudgetViewModel budgetViewModel);
    }

    public class UpdateBudgetCommand : IUpdateBudgetCommand
    {
        private readonly BudgetContext _budgetContext;
        private readonly BudgetMapper _budgetMapper;

        public UpdateBudgetCommand(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
            _budgetMapper = new BudgetMapper();
        }

        public async Task Execute(BudgetViewModel budgetViewModel)
        {
            var budget = await _budgetContext.Budgets
                .Where(b => b.Month == budgetViewModel.Month)
                .Where(b => b.Year == budgetViewModel.Year)
                .SingleAsync();

            _budgetMapper.Map(budgetViewModel, budget);
            await _budgetContext.SaveChangesAsync();
        }
    }
}