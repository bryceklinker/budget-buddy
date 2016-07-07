using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Add;
using BudgetBuddy.Api.Budgets.Exists;
using BudgetBuddy.Api.Budgets.Get;
using BudgetBuddy.Api.Budgets.Update;
using BudgetBuddy.Api.Budgets.ViewModels;
using BudgetBuddy.Api.General;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Budgets
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IGetBudgetQuery _getBudgetQuery;
        private readonly IAddBudgetCommand _addBudgetCommand;
        private readonly IUpdateBudgetCommand _updateBudgetCommand;
        private readonly IBudgetExistsQuery _budgetExistsQuery;

        public BudgetController(IDateTimeService dateTimeService, IGetBudgetQuery getBudgetQuery, IAddBudgetCommand addBudgetCommand, IUpdateBudgetCommand updateBudgetCommand, IBudgetExistsQuery budgetExistsQuery)
        {
            _dateTimeService = dateTimeService;
            _getBudgetQuery = getBudgetQuery;
            _addBudgetCommand = addBudgetCommand;
            _updateBudgetCommand = updateBudgetCommand;
            _budgetExistsQuery = budgetExistsQuery;
        }

        [HttpGet("current")]
        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> GetBudget(int? month = null, int? year = null)
        {
            month = month ?? _dateTimeService.CurrentMonth;
            year = year ?? _dateTimeService.CurrentYear;

            var exists = await _budgetExistsQuery.Execute(month.Value, year.Value);
            if (!exists)
                return NotFound();

            var budget = await _getBudgetQuery.Execute(month.Value, year.Value);
            return Ok(budget);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBudget(BudgetViewModel viewModel)
        {
            var newId = await _addBudgetCommand.Execute(viewModel);
            return Created($"~/budgets/{viewModel.Month}/{viewModel.Year}", newId);
        }

        [HttpPut("{month:int}/{year:int}")]
        public async Task<IActionResult> UpdateBudget(int month, int year, BudgetViewModel viewModel)
        {
            var exists = await _budgetExistsQuery.Execute(month, year);
            if (!exists)
                return NotFound();

            await _updateBudgetCommand.Execute(viewModel);
            return Ok();
        }
    }
}