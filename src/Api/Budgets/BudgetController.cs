using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Queries.GetBudget;
using BudgetBuddy.Api.General;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Budgets
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IGetBudgetQuery _getBudgetQuery;

        public BudgetController(IDateTimeService dateTimeService, IGetBudgetQuery getBudgetQuery)
        {
            _dateTimeService = dateTimeService;
            _getBudgetQuery = getBudgetQuery;
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
    }
}