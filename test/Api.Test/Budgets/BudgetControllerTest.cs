using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.Add;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.Exists;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.Get;
using BudgetBuddy.Test.Utilities.Stubs.Budgets.Update;
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
        private readonly UpdateBudgetCommandStub _updateBudgetCommandStub;
        private readonly BudgetExistsQueryStub _budgetExistsQueryStub;

        public BudgetControllerTest()
        {
            _dateTimeServiceStub = new DateTimeServiceStub();
            _getBudgetQueryStub = new GetBudgetQueryStub();
            _addBudgetCommandStub = new AddBudgetCommandStub();
            _updateBudgetCommandStub = new UpdateBudgetCommandStub();
            _budgetExistsQueryStub = new BudgetExistsQueryStub {Exists = true};

            _budgetController = new BudgetController(_dateTimeServiceStub, _getBudgetQueryStub, _addBudgetCommandStub, _updateBudgetCommandStub, _budgetExistsQueryStub);
        }

        [Fact]
        public async Task GetBudget_ShouldGetBudgetForCurrentMonth()
        {
            _dateTimeServiceStub.Now = new DateTime(2016, 6, 1);

            await _budgetController.GetBudget();
            Assert.Equal(6, _getBudgetQueryStub.Month);
        }

        [Fact]
        public async Task GetBudget_ShouldGetBudgetForCurrentYear()
        {
            _dateTimeServiceStub.Now = new DateTime(2016, 2, 1);

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
            _budgetExistsQueryStub.Exists = false;

            var result = await _budgetController.GetBudget();
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(_dateTimeServiceStub.Now.Month, _budgetExistsQueryStub.Month);
            Assert.Equal(_dateTimeServiceStub.Now.Year, _budgetExistsQueryStub.Year);
        }

        [Fact]
        public async Task AddBudget_ShouldReturnNewBudgetId()
        {
            var viewModel = new BudgetViewModel { StartDate = new DateTime(2015, 4, 1)};

            var result = (CreatedResult)await _budgetController.AddBudget(viewModel);
            AssertBudgetAdded(result, viewModel);
        }

        [Fact]
        public async Task UpdateBudget_ShouldBeOk()
        {
            _budgetExistsQueryStub.Exists = true;
            var viewModel = new BudgetViewModel();

            var result = await _budgetController.UpdateBudget(4, 2012, viewModel);
            Assert.Same(viewModel, _updateBudgetCommandStub.BudgetViewModel);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateBudget_ShouldBeNotFound()
        {
            _budgetExistsQueryStub.Exists = false;

            var result = await _budgetController.UpdateBudget(5, 2011, new BudgetViewModel());
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(5, _budgetExistsQueryStub.Month);
            Assert.Equal(2011, _budgetExistsQueryStub.Year);
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

        [Fact]
        public void AddBudget_ShouldGetBudgetFromBody()
        {
            var fromBody = _budgetController.GetType()
                .GetTypeInfo()
                .GetMethod("AddBudget")
                .GetParameters()
                .Last()
                .CustomAttributes
                .First();

            Assert.NotNull(fromBody);
        }

        [Fact]
        public void UpdateBudget_ShouldAllowHttpPut()
        {
            var httpPut = _budgetController.GetAttribute<HttpPutAttribute>("UpdateBudget");
            Assert.Equal("{month:int}/{year:int}", httpPut.Template);
        }

        [Fact]
        public void UpdateBudget_ShouldGetBudgetFromBody()
        {
            var fromBody = _budgetController.GetType()
                .GetMethod("AddBudget")
                .GetParameters()
                .Last()
                .CustomAttributes
                .First();

            Assert.NotNull(fromBody);
        }

        private void AssertBudgetAdded(CreatedResult result, BudgetViewModel viewModel)
        {
            Assert.Equal("~/budgets/4/2015", result.Location);
            Assert.Equal(_addBudgetCommandStub.NewId, result.Value);
            Assert.Same(viewModel, _addBudgetCommandStub.AddedBudget);
        }
    }
}
