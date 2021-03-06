﻿using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Shared.Asserts
{
    public static class BudgetAssert
    {
        public static void Equal(BudgetViewModel viewModel, Budget budget)
        {
            Assert.Equal(viewModel.Month, budget.StartDate.Month);
            Assert.Equal(viewModel.Year, budget.StartDate.Year);
            Assert.Equal(viewModel.Income, budget.Income);
            Assert.Equal(viewModel.Categories.Length, budget.Categories.Count);
            foreach (var categoryViewModel in viewModel.Categories)
            {
                var category = budget.Categories.Single(c => c.Name == categoryViewModel.Name);
                CategoryAssert.Equal(categoryViewModel, category);
            }
        }
    }
}
