using System.Threading.Tasks;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Mappers;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.Budgets.Update
{
    public interface IUpdateBudgetCommand
    {
        Task Execute(BudgetViewModel budgetViewModel);
    }

    [Transient(typeof(IUpdateBudgetCommand))]
    public class UpdateBudgetCommand : IUpdateBudgetCommand
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly BudgetMapper _budgetMapper;

        public UpdateBudgetCommand(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
            _budgetMapper = new BudgetMapper();
        }

        public async Task Execute(BudgetViewModel budgetViewModel)
        {
            var budgets = await _budgetRepository.GetAll();
            var budget = budgets
                .Where(b => b.StartDate.Month == budgetViewModel.Month)
                .Single(b => b.StartDate.Year == budgetViewModel.Year);

            _budgetMapper.Map(budgetViewModel, budget);
            await _budgetRepository.Update(budget);
        }
    }
}