using System;
using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.AddBudget;
using BudgetBuddy.Core.Budgets.GetBudget;
using BudgetBuddy.Core.Budgets.ViewModels;
using BudgetBuddy.Core.General;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Budgets
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IGetBudgetQuery _getBudgetQuery;
        private readonly IAddBudgetCommand _addBudgetCommand;

        public BudgetController(IDateTimeService dateTimeService, IGetBudgetQuery getBudgetQuery, IAddBudgetCommand addBudgetCommand)
        {
            _dateTimeService = dateTimeService;
            _getBudgetQuery = getBudgetQuery;
            _addBudgetCommand = addBudgetCommand;
        }

        [HttpGet("current")]
        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> GetBudget(int? month = null, int? year = null)
        {
            month = month ?? _dateTimeService.CurrentMonth;
            year = year ?? _dateTimeService.CurrentYear;
            var budget = await _getBudgetQuery.Execute(month.Value, year.Value);
            if (budget == null)
                return NotFound();
            return Ok(budget);
        }

        public async Task<IActionResult> AddBudget(BudgetViewModel viewModel)
        {
            var newId = await _addBudgetCommand.Execute(viewModel);
            return Created($"~/budgets/{viewModel.Month}/{viewModel.Year}", newId);
        }
    }
}