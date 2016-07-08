using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Update;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using Xunit;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;

namespace BudgetBuddy.Api.Test.Budgets.Update
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
                StartDate = new DateTime(2014, 8, 1),
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
                StartDate = new DateTime(_budgetViewModel.Year, _budgetViewModel.Month, 1),
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
