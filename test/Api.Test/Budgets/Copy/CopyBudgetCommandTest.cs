using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Factories;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets.Copy
{
    public class CopyBudgetCommandTest
    {
        private readonly BudgetContext _budgetContext;
        private readonly DateTimeServiceStub _dateTimeServiceStub;
        private readonly CopyBudgetCommand _copyBudgetCommand;

        public CopyBudgetCommandTest()
        {
            _budgetContext = DbContextFactory.Create<BudgetContext>();
            _dateTimeServiceStub = new DateTimeServiceStub();

            _copyBudgetCommand = new CopyBudgetCommand(_budgetContext, _dateTimeServiceStub);
        }

        [Fact]
        public async Task Execute_ShouldCreateBudgetForNextMonth()
        {
            _budgetContext.Add(CreateBudget(new DateTime(2015, 4, 1)));
            _budgetContext.Add(CreateBudget(new DateTime(2015, 5, 1)));
            _budgetContext.Add(CreateBudget(new DateTime(2015, 3, 1)));
            await _budgetContext.SaveChangesAsync();

            await _copyBudgetCommand.Execute();
            Assert.Equal(new DateTime(2015, 6, 1), _budgetContext.Budgets.OrderByDescending(b => b.StartDate).First().StartDate);
        }

        [Fact]
        public async Task Execute_ShouldCreateBudgetForCurrentMonthAndYear()
        {
            _budgetContext.RemoveRange(_budgetContext.Budgets);
            await _budgetContext.SaveChangesAsync();

            _dateTimeServiceStub.Now = new DateTime(2016, 3, 4);

            await _copyBudgetCommand.Execute();
            Assert.Equal(new DateTime(2016, 3, 1), _budgetContext.Budgets.OrderByDescending(b => b.StartDate).First().StartDate);
        }

        [Fact]
        public async Task Execute_ShouldCopyLineItemsFromExistingBudget()
        {
            var budget = CreatePopulatedBudget();
            _budgetContext.Add(budget);
            await _budgetContext.SaveChangesAsync();

            await _copyBudgetCommand.Execute();
            var newBudget = _budgetContext.Budgets.OrderByDescending(b => b.StartDate).First();
            AssertBudgetEqual(budget, newBudget);
        }

        [Fact]
        public void CopyBudgetCommand_ShouldBeTransient()
        {
            var transient = _copyBudgetCommand.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(ICopyBudgetCommand), transient.InterfaceType);
        }

        [Fact]
        public void Job_ShouldUseCopyCommand()
        {
            Assert.Equal(typeof(ICopyBudgetCommand), CopyBudgetCommand.Job.Type);
        }

        [Fact]
        public void Job_ShouldUseExecuteMethod()
        {
            Assert.Equal("Execute", CopyBudgetCommand.Job.Method.Name);
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
                LineItems = new List<BudgetLineItem>
                {
                    new BudgetLineItem
                    {
                        Name = "Mortgage",
                        Estimate = 23.12m,
                        Actual = 43.23m,
                        Category = new Category {Name = "Home"}
                    },
                    new BudgetLineItem
                    {
                        Name = "Student Loan",
                        Estimate = 23.12m,
                        Actual = 43.23m,
                        Category = new Category {Name = "Education"}
                    },
                    new BudgetLineItem
                    {
                        Name = "Restraunts",
                        Estimate = 23.12m,
                        Actual = 43.23m,
                        Category = new Category {Name = "Entertainment"}
                    }
                }
            };
        }

        private void AssertBudgetEqual(Budget expected, Budget actual)
        {
            Assert.Equal(expected.StartDate.AddMonths(1), actual.StartDate);
            Assert.NotEqual(expected.Id, actual.Id);
            Assert.Equal(expected.LineItems.Count, actual.LineItems.Count);
            foreach (var expectedLineItem in expected.LineItems)
            {
                var actualLineItem = actual.LineItems
                    .Where(l => l.Name == expectedLineItem.Name)
                    .Single(l => l.Category.Id == expectedLineItem.Category.Id);
                AssertBudgetLineItemEqual(expectedLineItem, actualLineItem);
            }
        }

        private void AssertBudgetLineItemEqual(BudgetLineItem expected, BudgetLineItem actual)
        {
            Assert.NotEqual(expected.Budget.Id, actual.Budget.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Category.Id, actual.Category.Id);
            Assert.Equal(expected.Estimate, actual.Estimate);
            Assert.Equal(0, actual.Actual);
        }
    }
}
