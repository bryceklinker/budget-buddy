using System.Linq;
using BudgetBuddy.Api.Budgets.Model.Entities;
using BudgetBuddy.Api.Budgets.ViewModels;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Asserts
{
    public static class BudgetAssert
    {
        public static void Equal(BudgetViewModel viewModel, Budget budget)
        {
            Assert.Equal(viewModel.Month, budget.Month);
            Assert.Equal(viewModel.Year, budget.Year);
            Assert.Equal(viewModel.Categories.Length, budget.LineItems.GroupBy(l => l.Category.Id).Count());
            foreach (var categoryViewModel in viewModel.Categories)
                BudgetLineItemAssert.Equal(categoryViewModel, budget.LineItems);
        }
    }
}
