using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Add;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;
using BudgetBuddy.Test.Utilities.Stubs.General;
using System.Linq;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Add
{
    
    public class AddBudgetCommandTest
    {
        private readonly BudgetViewModel _budgetViewModel;
        private readonly InMemoryRepository<Budget> _budgetRepository;
        private readonly AddBudgetCommand _addBudgetCommand;

        public AddBudgetCommandTest()
        {
            _budgetViewModel = new BudgetViewModel {Categories = new BudgetCategoryViewModel[0]};
            _budgetRepository = new InMemoryRepository<Budget>();
            _addBudgetCommand = new AddBudgetCommand(_budgetRepository);
        }

        [Fact]
        public async Task Execute_ShouldAddBudget()
        {
            _budgetViewModel.StartDate = new DateTime(2015, 6, 1);
            _budgetViewModel.Income = 556.23m;

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
        public async Task Execute_ShouldThrowInvalidOperation()
        {
            await _budgetRepository.Insert(new Budget {StartDate = new DateTime(9, 4, 1)});

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
            var budget = _budgetRepository.Entities.Single(b => b.Id == budgetId);
            BudgetAssert.Equal(viewModel, budget);
        }
    }
}
