using System.Threading.Tasks;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Mappers;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Update
{
    public interface IUpdateBudgetCommand
    {
        Task Execute(BudgetViewModel budgetViewModel);
    }

    [Transient(typeof(IUpdateBudgetCommand))]
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
                .Where(b => b.StartDate.Month == budgetViewModel.Month)
                .Where(b => b.StartDate.Year == budgetViewModel.Year)
                .SingleAsync();

            _budgetMapper.Map(budgetViewModel, budget);
            await _budgetContext.SaveChangesAsync();
        }
    }
}