using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Get;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Test.Budgets.Shared.Asserts;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Xunit;


namespace BudgetBuddy.Api.Test.Budgets.Get
{
    
    public class GetBudgetQueryTest
    {
        private readonly Budget _budget;
        private readonly GetBudgetQuery _getBudgetQuery;
        private readonly InMemoryRepository<Budget> _budgetRepository;

        public GetBudgetQueryTest()
        {
            _budget = new Budget { StartDate = new DateTime(2015, 6, 1), Income = 34.123m, Categories = new List<Category>() };
            _budgetRepository = new InMemoryRepository<Budget>();
            _budgetRepository.Insert(_budget).Wait();

            _getBudgetQuery = new GetBudgetQuery(_budgetRepository);
        }

        [Fact]
        public async Task Execute_ShouldGetBudgetForMonthAndYear()
        {
            await _budgetRepository.Insert(new Budget { StartDate = new DateTime(2015, 5, 1), Income = 343.123m });
            await _budgetRepository.Insert(new Budget { StartDate = new DateTime(2014, 6, 1), Income = 3123.32m });

            var viewModel = await Execute();
            Assert.Equal(2015, viewModel.Year);
            Assert.Equal(6, viewModel.Month);
            Assert.Equal(34.123m, viewModel.Income);
        }

        [Fact]
        public async Task Execute_ShouldGetBudgetLineItemsByCategory()
        {
            _budget.Categories = new List<Category>
            {
                new Category
                {
                    Name = "Misc",
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem {Name = "Home"},
                        new BudgetLineItem {Name = "Misc"}
                    }
                },
                new Category
                {
                    Name = "Food",
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem {Name = "Home"}
                    }
                },
                new Category
                {
                    Name = "House",
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem {Name = "Home"}
                    }
                }
            };

            var viewModel = await Execute();
            Assert.Equal(3, viewModel.Categories.Length);
        }

        [Fact]
        public async Task Execute_ShouldGetCategoryValues()
        {
            _budget.Categories = new List<Category>
            {
                new Category
                {
                    Name = "Bill",
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem {Name = "Jack"}
                    }
                }
            };

            var viewModel = await Execute();
            CategoryAssert.Equal(_budget.Categories[0], viewModel.Categories[0]);
        }

        [Fact]
        public async Task Execute_ShouldGetLineItemsForCategory()
        {
            _budget.Categories = new List<Category>
            {
                new Category
                {
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem(),
                        new BudgetLineItem(),
                        new BudgetLineItem()
                    }
                }
            };

            var viewModel = await Execute();
            Assert.Equal(3, viewModel.Categories[0].LineItems.Length);
        }

        [Fact]
        public async Task Execute_ShouldGetLineItemValues()
        {
            _budget.Categories = new List<Category>
            {
                new Category
                {
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem
                        {
                            Actual = 34.13m,
                            Estimate = 43.123m,
                            Name = "good"
                        }
                    }
                }
            };

            var viewModel = await Execute();
            BudgetLineItemAssert.Equal(_budget.Categories[0].BudgetLineItems[0], viewModel.Categories[0].LineItems[0]);
        }

        [Fact]
        public async Task Execute_ShouldGetNull()
        {
            var viewModel = await _getBudgetQuery.Execute(2016, 4);
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
            return _getBudgetQuery.Execute(_budget.StartDate.Year, _budget.StartDate.Month);
        }
    }
}
