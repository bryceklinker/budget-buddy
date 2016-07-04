using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Mappers;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.Model.Entities;
using BudgetBuddy.Core.Budgets.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Core.Budgets.AddBudget
{
    public interface IAddBudgetCommand
    {
        Task<Guid> Execute(BudgetViewModel budgetViewModel);
    }

    [Transient(typeof(IAddBudgetCommand))]
    public class AddBudgetCommand : IAddBudgetCommand
    {
        private readonly BudgetContext _budgetContext;
        private readonly BudgetMapper _budgetMapper;

        public AddBudgetCommand(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
            _budgetMapper = new BudgetMapper();
        }

        public async Task<Guid> Execute(BudgetViewModel budgetViewModel)
        {
            if (await IsExistingBudget(budgetViewModel))
            {
                throw new InvalidOperationException($"Budget for {budgetViewModel.Month}/{budgetViewModel.Year} already exists");
            }

            var budget = CreateBudget(budgetViewModel);
            _budgetContext.Add(budget);
            await _budgetContext.SaveChangesAsync();
            return budget.Id;
        }

        private Task<bool> IsExistingBudget(BudgetViewModel viewModel)
        {
            return _budgetContext.Budgets
                .Where(b => b.Month == viewModel.Month)
                .Where(b => b.Year == viewModel.Year)
                .AnyAsync();
        }

        private Budget CreateBudget(BudgetViewModel viewModel)
        {
            var budget = new Budget {LineItems = new List<BudgetLineItem>()};
            _budgetMapper.Map(viewModel, budget);
            return budget;
        }
    }
}