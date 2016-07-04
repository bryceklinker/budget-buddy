using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.Model.Entities;
using BudgetBuddy.Core.Budgets.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Core.Budgets.AddBudget
{
    public interface IAddBudgetCommand
    {
        Task<Guid> Execute(BudgetViewModel budgetViewModel);
    }

    public class AddBudgetCommand : IAddBudgetCommand
    {
        private readonly BudgetContext _budgetContext;

        public AddBudgetCommand(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
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
            return new Budget
            {
                Month = viewModel.Month,
                Year = viewModel.Year,
                LineItems = CreateLineItems(viewModel.Categories).ToList()
            };
        }

        private IEnumerable<BudgetLineItem> CreateLineItems(IEnumerable<BudgetCategoryViewModel> categories)
        {
            return categories.SelectMany(CreateLineItems);
        }

        private IEnumerable<BudgetLineItem> CreateLineItems(BudgetCategoryViewModel category)
        {
            return category.LineItems.Select(l => CreateLineItem(l, category));
        }

        private BudgetLineItem CreateLineItem(BudgetLineItemViewModel lineItem, BudgetCategoryViewModel category)
        {
            return new BudgetLineItem
            {
                Actual = lineItem.Actual,
                Estimate = lineItem.Estimate,
                Name = lineItem.Name,
                Category = CreateCategory(category)
            };
        }

        private Category CreateCategory(BudgetCategoryViewModel category)
        {
            return new Category
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}