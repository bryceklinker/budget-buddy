using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Shared.Asserts
{
    public static class CategoryAssert
    {
        public static void Equal(BudgetCategoryViewModel viewModel, Category category)
        {
            Assert.Equal(viewModel.Name, category.Name);
            Assert.Equal(viewModel.LineItems.Length, category.BudgetLineItems.Count);

            foreach (var itemViewModel in viewModel.LineItems)
            {
                var lineItem = category.BudgetLineItems.Single(b => b.Name == itemViewModel.Name);
                BudgetLineItemAssert.Equal(itemViewModel, lineItem);
            }
        }

        public static void Equal(Category category, BudgetCategoryViewModel viewModel)
        {
            Assert.Equal(category.Name, viewModel.Name);
            Assert.Equal(category.BudgetLineItems.Count, viewModel.LineItems.Length);

            foreach (var itemViewModel in category.BudgetLineItems)
            {
                var lineItem = viewModel.LineItems.Single(b => b.Name == itemViewModel.Name);
                BudgetLineItemAssert.Equal(itemViewModel, lineItem);
            }
        }

        public static void EqualWithoutActuals(Category expected, Category actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.BudgetLineItems.Count, actual.BudgetLineItems.Count);

            foreach (var budgetLineItem in expected.BudgetLineItems)
            {
                var actualLineItem = actual.BudgetLineItems.Single(b => b.Name == budgetLineItem.Name);
                BudgetLineItemAssert.EqualWithoutActuals(budgetLineItem, actualLineItem);
            }
        }
    }
}