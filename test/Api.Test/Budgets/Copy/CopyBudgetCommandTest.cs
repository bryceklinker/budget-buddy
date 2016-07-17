using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.General;
using System.Linq;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Copy
{
    
    public class CopyBudgetCommandTest
    {
        private readonly InMemoryRepository<Budget> _budgetRepository;
        private readonly DateTimeServiceStub _dateTimeServiceStub;
        private readonly CopyBudgetCommand _copyBudgetCommand;

        public CopyBudgetCommandTest()
        {
            _budgetRepository = new InMemoryRepository<Budget>();
            _dateTimeServiceStub = new DateTimeServiceStub();

            _copyBudgetCommand = new CopyBudgetCommand(_budgetRepository, _dateTimeServiceStub);
        }

        [Fact]
        public async Task Execute_ShouldCreateBudgetForNextMonth()
        {
            await _budgetRepository.Insert(CreateBudget(new DateTime(2015, 4, 1)));
            await _budgetRepository.Insert(CreateBudget(new DateTime(2015, 5, 1)));
            await _budgetRepository.Insert(CreateBudget(new DateTime(2015, 3, 1)));

            await _copyBudgetCommand.Execute();
            Assert.Equal(new DateTime(2015, 6, 1), _budgetRepository.Entities.OrderByDescending(b => b.StartDate).First().StartDate);
        }

        [Fact]
        public async Task Execute_ShouldCreateBudgetForCurrentMonthAndYear()
        {
            _budgetRepository.Entities.Clear();

            _dateTimeServiceStub.Now = new DateTime(2016, 3, 4);

            await _copyBudgetCommand.Execute();
            Assert.Equal(new DateTime(2016, 3, 1), _budgetRepository.Entities.OrderByDescending(b => b.StartDate).First().StartDate);
        }

        [Fact]
        public async Task Execute_ShouldCopyLineItemsFromExistingBudget()
        {
            var budget = CreatePopulatedBudget();
            await _budgetRepository.Insert(budget);

            await _copyBudgetCommand.Execute();
            var newBudget = _budgetRepository.Entities.OrderByDescending(b => b.StartDate).First();
            BudgetAssert.EqualWithoutActuals(budget, newBudget);
        }

        [Fact]
        public async Task Execute_ShouldNotCopyBudgetForNextMonthIfNextMonthsBudgetAlreadyExists()
        {
            _dateTimeServiceStub.Now = new DateTime(2016, 6, 1);
            await _budgetRepository.Insert(CreateBudget(new DateTime(2016, 7, 1)));

            await _copyBudgetCommand.Execute();
            Assert.Equal(1, _budgetRepository.Entities.Count);
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
            var job = CopyBudgetCommand.CreateCopyJob();
            Assert.Equal(typeof(ICopyBudgetCommand), job.Type);
        }

        [Fact]
        public void Job_ShouldUseExecuteMethod()
        {
            var job = CopyBudgetCommand.CreateCopyJob();
            Assert.Equal("Execute", job.Method.Name);
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
    }
}
