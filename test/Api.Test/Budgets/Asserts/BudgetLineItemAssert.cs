using System.Collections.Generic;
using System.Linq;
using BudgetBuddy.Api.Budgets.Model.Entities;
using BudgetBuddy.Api.Budgets.ViewModels;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Asserts
{
    public static class BudgetLineItemAssert
    {
        public static void Equal(BudgetCategoryViewModel viewModel, IEnumerable<BudgetLineItem> budgetLineItems)
        {
            var categoryLineItems = budgetLineItems.Where(l => l.Category.Name == viewModel.Name).ToList();
            Assert.Equal(viewModel.LineItems.Length, categoryLineItems.Count);
            foreach (var lineItemViewModel in viewModel.LineItems)
            {
                var lineItem = categoryLineItems.Single(l => l.Name == lineItemViewModel.Name);
                Equal(lineItemViewModel, lineItem);
            }
        }

        public static void Equal(BudgetLineItemViewModel viewModel, BudgetLineItem lineItem)
        {
            Assert.Equal(viewModel.Actual, lineItem.Actual);
            Assert.Equal(viewModel.Estimate, lineItem.Estimate);
            Assert.Equal(viewModel.Name, lineItem.Name);
        }
    }
}
