using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Update;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Update
{
    
    public class UpdateBudgetCommandTest
    {
        private readonly BudgetViewModel _budgetViewModel;
        private readonly Budget _budget;
        private readonly InMemoryRepository<Budget> _budgetRepository;
        private readonly UpdateBudgetCommand _updateBudgetCommand;

        public UpdateBudgetCommandTest()
        {
            _budgetViewModel = new BudgetViewModel
            {
                Income = 45.3m,
                StartDate = new DateTime(2014, 8, 1),
                Categories = new[]
                {
                    new BudgetCategoryViewModel
                    {
                        Name = "Home",
                        LineItems = new[]
                        {
                            new BudgetLineItemViewModel {Actual = 54, Estimate = 23, Name = "Mortgage"}
                        }
                    }
                }
            };
            _budget = new Budget
            {
                StartDate = new DateTime(_budgetViewModel.Year, _budgetViewModel.Month, 1),
                Categories = new List<Category>()
            };
            _budgetRepository = new InMemoryRepository<Budget>();
            _budgetRepository.Insert(_budget).Wait();

            _updateBudgetCommand = new UpdateBudgetCommand(_budgetRepository);
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
            _budget.Categories.Add(new Category
            {
                BudgetLineItems = new List<BudgetLineItem>
                {
                    new BudgetLineItem
                    {
                        Name = _budgetViewModel.Categories[0].LineItems[0].Name,
                        Actual = 34,
                        Estimate = 54,
                    }
                }
            });

            await _updateBudgetCommand.Execute(_budgetViewModel);
            BudgetAssert.Equal(_budgetViewModel, _budget);
            Assert.Equal(1, _budgetRepository.Entities.Count);
        }

        [Fact]
        public void UpdateBudgetCommand_ShouldBeTransient()
        {
            var transient = _updateBudgetCommand.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(IUpdateBudgetCommand), transient.InterfaceType);
        }
    }
}
