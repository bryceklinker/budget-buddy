﻿using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Add;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;

namespace BudgetBuddy.Api.Test.Budgets.Add
{
    public class AddBudgetCommandTest
    {
        private readonly BudgetViewModel _budgetViewModel;
        private readonly BudgetContext _budgetContext;
        private readonly AddBudgetCommand _addBudgetCommand;

        public AddBudgetCommandTest()
        {
            _budgetViewModel = new BudgetViewModel {Categories = new BudgetCategoryViewModel[0]};
            _budgetContext = DbContextFactory.Create<BudgetContext>();
            _addBudgetCommand = new AddBudgetCommand(_budgetContext);
        }

        [Fact]
        public async Task Execute_ShouldAddBudget()
        {
            _budgetViewModel.StartDate = new DateTime(2015, 6, 1);

            var id = await _addBudgetCommand.Execute(_budgetViewModel);
            AssertAddedBudget(_budgetViewModel, id);
        }

        [Fact]
        public async Task Execute_ShouldAddBudgetLineItems()
        {
            _budgetViewModel.Categories = new[]
            {
                new BudgetCategoryViewModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Home",
                    LineItems = new[]
                    {
                        new BudgetLineItemViewModel {Actual = 3.2m, Estimate = 6.4m, Name = "Misc. Deck"}
                    }
                }
            };

            var id = await _addBudgetCommand.Execute(_budgetViewModel);
            AssertAddedBudget(_budgetViewModel, id);
        }

        [Fact]
        public async Task Execute_ShouldUseExistingCategory()
        {
            var existingCategory = new Category {Id = Guid.NewGuid(), Name = "Something"};
            _budgetContext.Add(existingCategory);
            await _budgetContext.SaveChangesAsync();

            _budgetViewModel.Categories = new[]
            {
                new BudgetCategoryViewModel
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name,
                    LineItems = new[]
                    {
                        new BudgetLineItemViewModel {Actual = 5, Estimate = 8, Name = "Home"}
                    }
                }
            };

            await _addBudgetCommand.Execute(_budgetViewModel);
            Assert.Equal(1, _budgetContext.Categories.Count());
        }

        [Fact]
        public async Task Execute_ShouldThrowInvalidOperation()
        {
            _budgetContext.Add(new Budget {StartDate = new DateTime(9, 4, 1)});
            await _budgetContext.SaveChangesAsync();

            try
            {
                _budgetViewModel.StartDate = new DateTime(9, 4, 1);
                await _addBudgetCommand.Execute(_budgetViewModel);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Equal("Budget for 4/9 already exists", ex.Message);
            }
        }

        [Fact]
        public void AddBudgetCommand_ShouldBeTransient()
        {
            var registration = _addBudgetCommand.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IAddBudgetCommand), registration.InterfaceType);
        }

        private void AssertAddedBudget(BudgetViewModel viewModel, Guid budgetId)
        {
            var budget = _budgetContext.Budgets.Single(b => b.Id == budgetId);
            BudgetAssert.Equal(viewModel, budget);
        }
    }
}
