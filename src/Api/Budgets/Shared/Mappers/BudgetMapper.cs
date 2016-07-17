using System;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;

namespace BudgetBuddy.Api.Budgets.Shared.Mappers
{
    public class BudgetMapper
    {
        public void Map(BudgetViewModel viewModel, Budget budget)
        {
            budget.StartDate = new DateTime(viewModel.Year, viewModel.Month, 1);
            budget.Income = viewModel.Income;
            budget.Categories = viewModel.Categories.Select(Map).ToList();
        }

        private static Category Map(BudgetCategoryViewModel viewModel)
        {
            return new Category
            {
                Name = viewModel.Name,
                BudgetLineItems = viewModel.LineItems.Select(Map).ToList()
            };
        }

        private static BudgetLineItem Map(BudgetLineItemViewModel viewModel)
        {
            return new BudgetLineItem
            {
                Name = viewModel.Name,
                Actual = viewModel.Actual,
                Estimate = viewModel.Estimate
            };
        }
    }
}
