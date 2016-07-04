using System;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.AddBudget;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.Model.Entities;
using BudgetBuddy.Core.Budgets.ViewModels;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;
using System.Linq;

namespace BudgetBuddy.Core.Test.Budgets.AddBudget
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
            _budgetViewModel.Month = 6;
            _budgetViewModel.Year = 2015;

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
            _budgetContext.Add(new Budget { Month = 4, Year = 9 });
            await _budgetContext.SaveChangesAsync();

            try
            {
                _budgetViewModel.Month = 4;
                _budgetViewModel.Year = 9;    
                await _addBudgetCommand.Execute(_budgetViewModel);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Equal("Budget for 4/9 already exists", ex.Message);
            }
        }

        private void AssertAddedBudget(BudgetViewModel viewModel, Guid budgetId)
        {
            var budget = _budgetContext.Budgets.Single(b => b.Id == budgetId);
            Assert.Equal(viewModel.Month, budget.Month);
            Assert.Equal(viewModel.Year, budget.Year);
            Assert.Equal(viewModel.Categories.Length, budget.LineItems.GroupBy(l => l.Category.Id).Count());
            foreach (var categoryViewModel in viewModel.Categories)
                AssertBudgetLineItems(categoryViewModel, budget);
        }

        private void AssertBudgetLineItems(BudgetCategoryViewModel viewModel, Budget budget)
        {
            var categoryLineItems = budget.LineItems.Where(l => l.Category.Name == viewModel.Name).ToList();
            Assert.Equal(viewModel.LineItems.Length, categoryLineItems.Count);
            foreach (var lineItemViewModel in viewModel.LineItems)
            {
                var lineItem = categoryLineItems.Single(l => l.Name == lineItemViewModel.Name);
                AssertBudgetLineItem(lineItemViewModel, lineItem);
            }
        }

        private void AssertBudgetLineItem(BudgetLineItemViewModel viewModel, BudgetLineItem lineItem)
        {
            Assert.Equal(viewModel.Actual, lineItem.Actual);
            Assert.Equal(viewModel.Estimate, lineItem.Estimate);
            Assert.Equal(viewModel.Name, lineItem.Name);
        }
    }
}
