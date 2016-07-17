using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Infrastructure.DependencyInjection;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Mappers;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Api.Budgets.Add
{
    public interface IAddBudgetCommand
    {
        Task<Guid> Execute(BudgetViewModel budgetViewModel);
    }

    [Transient(typeof(IAddBudgetCommand))]
    public class AddBudgetCommand : IAddBudgetCommand
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly BudgetMapper _budgetMapper;

        public AddBudgetCommand(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
            _budgetMapper = new BudgetMapper();
        }

        public async Task<Guid> Execute(BudgetViewModel budgetViewModel)
        {
            if (await IsExistingBudget(budgetViewModel))
                throw new InvalidOperationException($"Budget for {budgetViewModel.Month}/{budgetViewModel.Year} already exists");

            var budget = CreateBudget(budgetViewModel);
            await _budgetRepository.Insert(budget);
            return budget.Id;
        }

        private async Task<bool> IsExistingBudget(BudgetViewModel viewModel)
        {
            var budgets = await _budgetRepository.GetAll();
            return budgets
                .Where(b => b.StartDate.Month == viewModel.Month)
                .Any(b => b.StartDate.Year == viewModel.Year);
        }

        private Budget CreateBudget(BudgetViewModel viewModel)
        {
            var budget = new Budget();
            _budgetMapper.Map(viewModel, budget);
            return budget;
        }
    }
}