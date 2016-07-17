using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.General;
using System.Linq;
using BudgetBuddy.Api.Budgets.Copy.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Copy
{
    
    public class CopyBudgetCommandTest
    {
        private readonly InMemoryRepository<Budget> _budgetRepository;
        private readonly CopyBudgetCommand _copyBudgetCommand;
        private readonly CopyBudgetViewModel _copyBudgetViewModel;

        public CopyBudgetCommandTest()
        {
            _copyBudgetViewModel = new CopyBudgetViewModel();
            _budgetRepository = new InMemoryRepository<Budget>();

            _copyBudgetCommand = new CopyBudgetCommand(_budgetRepository);
        }

        [Fact]
        public async Task Execute_ShouldCopyLineItemsFromExistingBudget()
        {
            var budget = CreatePopulatedBudget();

            _copyBudgetViewModel.FromYear = 2016;
            _copyBudgetViewModel.FromMonth = 5;

            _copyBudgetViewModel.ToYear = 2016;
            _copyBudgetViewModel.ToMonth = 7;

            await _budgetRepository.Insert(budget);

            await _copyBudgetCommand.Execute(_copyBudgetViewModel);
            var newBudget = _budgetRepository.Entities
                .Where(b => b.StartDate.Year == 2016)
                .First(b => b.StartDate.Month == 7);

            AssertBudgetEqual(budget, newBudget, 2016, 7);
        }

        [Fact]
        public async Task Execute_ShouldThrowInvalidOperationIfBudgetAlreadyExists()
        {
            await _budgetRepository.Insert(CreateBudget(new DateTime(2016, 6, 1)));
            await _budgetRepository.Insert(CreateBudget(new DateTime(2016, 7, 1)));

            _copyBudgetViewModel.FromMonth = 6;
            _copyBudgetViewModel.FromYear = 2016;
            _copyBudgetViewModel.ToMonth = 7;
            _copyBudgetViewModel.ToYear = 2016;

            try
            {
                await _copyBudgetCommand.Execute(_copyBudgetViewModel);
                Assert.False(true); // Ensure test fails if exception is not thrown.
            }
            catch (Exception ex)
            {
                Assert.Equal(2, _budgetRepository.Entities.Count);
                Assert.IsType<InvalidOperationException>(ex);
            }
            
        }

        [Fact]
        public void CopyBudgetCommand_ShouldBeTransient()
        {
            var transient = _copyBudgetCommand.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(ICopyBudgetCommand), transient.InterfaceType);
        }

        [Fact]
        public void JobId_ShouldBeBudgetCopyOrCreate()
        {
            Assert.Equal("budgets-copy-or-create-next", CopyBudgetCommand.JobId);
        }

        private static Budget CreateBudget(DateTime startDate)
        {
            return new Budget
            {
                StartDate = startDate
            };
        }

        private static Budget CreatePopulatedBudget()
        {
            return new Budget
            {
                StartDate = new DateTime(2016, 5, 1),
                Categories = new List<Category>
                {
                    new Category
                    {
                        BudgetLineItems = new List<BudgetLineItem>
                        {
                            new BudgetLineItem
                            {
                                Name = "Mortgage",
                                Estimate = 23.12m,
                                Actual = 43.23m
                            },
                            new BudgetLineItem
                            {
                                Name = "Student Loan",
                                Estimate = 23.12m,
                                Actual = 43.23m,
                            },
                            new BudgetLineItem
                            {
                                Name = "Restraunts",
                                Estimate = 23.12m,
                                Actual = 43.23m,
                            }
                        }
                    }
                }
            };
        }

        private static void AssertBudgetEqual(Budget budget, Budget newBudget, int toYear, int toMonth)
        {
            Assert.Equal(new DateTime(toYear, toMonth, 1), newBudget.StartDate);
            Assert.NotEqual(budget.Id, newBudget.Id);
            Assert.Equal(budget.Categories.Count, newBudget.Categories.Count);

            foreach (var category in budget.Categories)
            {
                var actualCategory = newBudget.Categories.Single(c => c.Name == category.Name);
                CategoryAssert.EqualWithoutActuals(category, actualCategory);
            }
        }
    }
}
