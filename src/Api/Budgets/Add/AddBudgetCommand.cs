using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Mappers;
using BudgetBuddy.Api.Budgets.Model;
using BudgetBuddy.Api.Budgets.Model.Entities;
using BudgetBuddy.Api.Budgets.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Add
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