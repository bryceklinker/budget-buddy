using System.Collections.Generic;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Shared.Asserts
{
    public static class BudgetLineItemAssert
    {
        public static void Equal(BudgetLineItemViewModel viewModel, BudgetLineItem lineItem)
        {
            Assert.Equal(viewModel.Actual, lineItem.Actual);
            Assert.Equal(viewModel.Estimate, lineItem.Estimate);
            Assert.Equal(viewModel.Name, lineItem.Name);
        }

        public static void Equal(BudgetLineItem lineItem, BudgetLineItemViewModel viewModel)
        {
            Assert.Equal(lineItem.Actual, viewModel.Actual);
            Assert.Equal(lineItem.Estimate, viewModel.Estimate);
            Assert.Equal(lineItem.Name, viewModel.Name);
        }

        public static void EqualWithoutActuals(BudgetLineItem expected, BudgetLineItem actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Estimate, actual.Estimate);
        }
    }
}
