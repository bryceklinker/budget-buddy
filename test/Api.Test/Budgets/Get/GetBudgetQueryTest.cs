﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Get;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Get
{
    public class GetBudgetQueryTest
    {
        private readonly Budget _budget;
        private readonly GetBudgetQuery _getBudgetQuery;
        private readonly BudgetContext _budgetContext;

        public GetBudgetQueryTest()
        {
            _budget = new Budget { StartDate = new DateTime(2015, 6, 1), Income = 34.123m, LineItems = new List<BudgetLineItem>() };
            _budgetContext = DbContextFactory.Create<BudgetContext>();
            _budgetContext.Add(_budget);
            _budgetContext.SaveChanges();

            _getBudgetQuery = new GetBudgetQuery(_budgetContext);
        }

        [Fact]
        public async Task Execute_ShouldGetBudgetForMonthAndYear()
        {
            _budgetContext.Add(new Budget {StartDate = new DateTime(2015, 5, 1), Income = 343.123m});
            _budgetContext.Add(new Budget {StartDate = new DateTime(2014, 6, 1), Income = 3123.32m });
            await _budgetContext.SaveChangesAsync();

            var viewModel = await Execute();
            Assert.Equal(2015, viewModel.Year);
            Assert.Equal(6, viewModel.Month);
            Assert.Equal(34.123m, viewModel.Income);
        }

        [Fact]
        public async Task Execute_ShouldGetBudgetLineItemsByCategory()
        {
            var firstCategory = new Category {Id = Guid.NewGuid(), Name = "Misc"};
            var secondCategory = new Category {Id = Guid.NewGuid(), Name = "Food"};
            var thirdCategory = new Category {Id = Guid.NewGuid(), Name = "House"};
            _budget.LineItems.Add(new BudgetLineItem { Category = firstCategory, Name = "Home"});
            _budget.LineItems.Add(new BudgetLineItem { Category = secondCategory, Name = "Home"});
            _budget.LineItems.Add(new BudgetLineItem { Category = firstCategory, Name = "Misc"});
            _budget.LineItems.Add(new BudgetLineItem { Category = thirdCategory, Name = "Home"});
            await _budgetContext.SaveChangesAsync();

            var viewModel = await Execute();
            Assert.Equal(3, viewModel.Categories.Length);
        }

        [Fact]
        public async Task Execute_ShouldGetCategoryValues()
        {
            var category = new Category {Name = "Bill", Id = Guid.NewGuid()};
            _budget.LineItems.Add(new BudgetLineItem { Category = category, Name = "Jack"});
            await _budgetContext.SaveChangesAsync();

            var viewModel = await Execute();
            AssertCategoryEqual(category, viewModel.Categories[0]);
        }

        [Fact]
        public async Task Execute_ShouldGetLineItemsForCategory()
        {
            var category = new Category {Name = "Jack"};
            _budget.LineItems.Add(new BudgetLineItem {Category = category, Name = "Home"});
            _budget.LineItems.Add(new BudgetLineItem {Category = category, Name = "misc"});
            _budget.LineItems.Add(new BudgetLineItem {Category = category, Name = "Bob"});
            await _budgetContext.SaveChangesAsync();

            var viewModel = await Execute();
            Assert.Equal(3, viewModel.Categories[0].LineItems.Length);
        }

        [Fact]
        public async Task Execute_ShouldGetLineItemValues()
        {
            var lineItem = new BudgetLineItem
            {
                Actual = 34.13m,
                Estimate = 43.123m,
                Category = new Category {Name = "Home"},
                Id = Guid.NewGuid(),
                Name = "good"
            };
            _budget.LineItems.Add(lineItem);
            await _budgetContext.SaveChangesAsync();

            var viewModel = await Execute();
            AssertLineItemEqual(lineItem, viewModel.Categories[0].LineItems[0]);
        }

        [Fact]
        public async Task Execute_ShouldGetNull()
        {
            var viewModel = await _getBudgetQuery.Execute(4, 2016);
            Assert.Null(viewModel);
        }

        [Fact]
        public void Query_ShouldBeTransient()
        {
            var registration = _getBudgetQuery.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IGetBudgetQuery), registration.InterfaceType);
        }

        private Task<BudgetViewModel> Execute()
        {
            return _getBudgetQuery.Execute(_budget.StartDate.Month, _budget.StartDate.Year);
        }

        private void AssertCategoryEqual(Category category, BudgetCategoryViewModel viewModel)
        {
            Assert.Equal(category.Name, viewModel.Name);
            Assert.Equal(category.Id, viewModel.Id);
        }

        private void AssertLineItemEqual(BudgetLineItem lineItem, BudgetLineItemViewModel viewModel)
        {
            Assert.Equal(lineItem.Id, viewModel.Id);
            Assert.Equal(lineItem.Actual, viewModel.Actual);
            Assert.Equal(lineItem.Estimate, viewModel.Estimate);
        }
    }
}
