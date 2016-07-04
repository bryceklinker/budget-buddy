using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Model;
using BudgetBuddy.Core.Budgets.Model.Entities;
using BudgetBuddy.Core.Budgets.UpdateBudget;
using BudgetBuddy.Core.Budgets.ViewModels;
using BudgetBuddy.Core.Test.Budgets.Asserts;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;

namespace BudgetBuddy.Core.Test.Budgets.UpdateBudget
{
    public class UpdateBudgetCommandTest
    {
        private readonly BudgetViewModel _budgetViewModel;
        private readonly Budget _budget;
        private readonly BudgetContext _budgetContext;
        private readonly UpdateBudgetCommand _updateBudgetCommand;

        public UpdateBudgetCommandTest()
        {
            _budgetViewModel = new BudgetViewModel
            {
                Month = 8,
                Year = 2014,
                Categories = new[]
                {
                    new BudgetCategoryViewModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Home",
                        LineItems = new[]
                        {
                            new BudgetLineItemViewModel {Actual = 54, Estimate = 23, Name = "Mortgage", Id = Guid.NewGuid()}
                        }
                    }
                }
            };
            _budget = new Budget
            {
                Month = _budgetViewModel.Month,
                Year = _budgetViewModel.Year,
                LineItems = new List<BudgetLineItem>()
            };
            _budgetContext = DbContextFactory.Create<BudgetContext>();
            _budgetContext.Add(_budget);
            _budgetContext.SaveChanges();

            _updateBudgetCommand = new UpdateBudgetCommand(_budgetContext);
        }

        [Fact]
        public async Task Execute_ShouldAddLineItemsToBudget()
        {
            await _updateBudgetCommand.Execute(_budgetViewModel);
            BudgetAssert.Equal(_budgetViewModel, _budget);
        }

        [Fact]
        public async Task Execute_ShouldUpdateExistingLineItems()
        {
            _budget.LineItems.Add(new BudgetLineItem
            {
                Id = _budgetViewModel.Categories[0].LineItems[0].Id,
                Name = _budgetViewModel.Categories[0].LineItems[0].Name,
                Actual = 34,
                Estimate = 54,
                Category = new Category { Id = _budgetViewModel.Categories[0].Id, Name = _budgetViewModel.Categories[0].Name }
            });
            await _budgetContext.SaveChangesAsync();

            await _updateBudgetCommand.Execute(_budgetViewModel);
            BudgetAssert.Equal(_budgetViewModel, _budget);
            Assert.Equal(1, _budgetContext.BudgetLineItems.Count());
            Assert.Equal(1, _budgetContext.Categories.Count());
        }

        [Fact]
        public void UpdateBudgetCommand_ShouldBeTransient()
        {
            var transient = _updateBudgetCommand.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IUpdateBudgetCommand), transient.InterfaceType);
        }
    }
}
