using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets;
using BudgetBuddy.Core.Budgets.ViewModels;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.AddBudget;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.GetBudget;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BudgetBuddy.Api.Test.Budgets
{
    public class BudgetControllerTest
    {
        private readonly BudgetController _budgetController;
        private readonly DateTimeServiceStub _dateTimeServiceStub;
        private readonly GetBudgetQueryStub _getBudgetQueryStub;
        private readonly AddBudgetCommandStub _addBudgetCommandStub;

        public BudgetControllerTest()
        {
            _dateTimeServiceStub = new DateTimeServiceStub();
            _getBudgetQueryStub = new GetBudgetQueryStub();
            _addBudgetCommandStub = new AddBudgetCommandStub();

            _budgetController = new BudgetController(_dateTimeServiceStub, _getBudgetQueryStub, _addBudgetCommandStub);
        }

        [Fact]
        public async Task GetBudget_ShouldGetBudgetForCurrentMonth()
        {
            _dateTimeServiceStub.CurrentMonth = 6;

            await _budgetController.GetBudget();
            Assert.Equal(6, _getBudgetQueryStub.Month);
        }

        [Fact]
        public async Task GetBudget_ShouldGetBudgetForCurrentYear()
        {
            _dateTimeServiceStub.CurrentYear = 2016;

            await _budgetController.GetBudget();
            Assert.Equal(2016, _getBudgetQueryStub.Year);
        }

        [Fact]
        public async Task GetBudget_ShouldReturnBudget()
        {
            _getBudgetQueryStub.Result = new BudgetViewModel();

            var result = (OkObjectResult)await _budgetController.GetBudget();
            Assert.Same(_getBudgetQueryStub.Result, result.Value);
        }

        [Fact]
        public async Task GetBudget_ShouldGetBudgetForSpecifiedMonthAndYear()
        {
            await _budgetController.GetBudget(5, 2012);
            Assert.Equal(5, _getBudgetQueryStub.Month);
            Assert.Equal(2012, _getBudgetQueryStub.Year);
        }

        [Fact]
        public async Task GetBudget_ShouldBeNotFound()
        {
            _getBudgetQueryStub.Result = null;

            var result = await _budgetController.GetBudget();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddBudget_ShouldReturnNewBudgetId()
        {
            var viewModel = new BudgetViewModel {Month = 4, Year = 2015};

            var result = (CreatedResult)await _budgetController.AddBudget(viewModel);
            AssertBudgetAdded(result, viewModel);
        }

        [Fact]
        public void Controller_ShouldSpecifyRoute()
        {
            var route = _budgetController.GetAttribute<RouteAttribute>();
            Assert.Equal("budgets", route.Template);
        }

        [Fact]
        public void GetBudget_ShouldAllowHttpGetWithMonthAndYear()
        {
            var httpGets = _budgetController.GetAttributes<HttpGetAttribute>("GetBudget");
            Assert.True(httpGets.Any(a => a.Template == "{month:int}/{year:int}"));
        }

        [Fact]
        public void GetBudget_ShouldAllowHttpGetWithoutMonthAndYear()
        {
            var httpGets = _budgetController.GetAttributes<HttpGetAttribute>("GetBudget");
            Assert.True(httpGets.Any(a => a.Template == "current"));
        }

        [Fact]
        public void AddBudget_ShouldAllowHttpPost()
        {
            var httpPost = _budgetController.GetAttribute<HttpPostAttribute>("AddBudget");
            Assert.Equal("", httpPost.Template);
        }

        private void AssertBudgetAdded(CreatedResult result, BudgetViewModel viewModel)
        {
            Assert.Equal("~/budgets/4/2015", result.Location);
            Assert.Equal(_addBudgetCommandStub.NewId, result.Value);
            Assert.Same(viewModel, _addBudgetCommandStub.AddedBudget);
        }
    }
}
