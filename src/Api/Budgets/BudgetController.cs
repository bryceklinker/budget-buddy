using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Add;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Exists;
using BudgetBuddy.Api.Budgets.Get;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Budgets.Update;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Budgets
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private readonly IGetBudgetQuery _getBudgetQuery;
        private readonly IAddBudgetCommand _addBudgetCommand;
        private readonly IUpdateBudgetCommand _updateBudgetCommand;
        private readonly IBudgetExistsQuery _budgetExistsQuery;
        private readonly ICopyBudgetCommand _copyBudgetCommand;

        public BudgetController(IGetBudgetQuery getBudgetQuery, IAddBudgetCommand addBudgetCommand, IUpdateBudgetCommand updateBudgetCommand, IBudgetExistsQuery budgetExistsQuery, ICopyBudgetCommand copyBudgetCommand)
        {
            _getBudgetQuery = getBudgetQuery;
            _addBudgetCommand = addBudgetCommand;
            _updateBudgetCommand = updateBudgetCommand;
            _budgetExistsQuery = budgetExistsQuery;
            _copyBudgetCommand = copyBudgetCommand;
        }

        [HttpGet("{year:int}/{month:int}")]
        public async Task<IActionResult> GetBudget(int year, int month)
        {
            var exists = await _budgetExistsQuery.Execute(month, year);
            if (!exists)
                return NotFound();

            var budget = await _getBudgetQuery.Execute(year, month);
            return Ok(budget);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBudget([FromBody] BudgetViewModel viewModel)
        {
            var newId = await _addBudgetCommand.Execute(viewModel);
            return Created($"~/budgets/{viewModel.Month}/{viewModel.Year}", newId);
        }

        [HttpPut("{year:int}/{month:int}")]
        public async Task<IActionResult> UpdateBudget(int year, int month, [FromBody] BudgetViewModel viewModel)
        {
            var exists = await _budgetExistsQuery.Execute(month, year);
            if (!exists)
                return NotFound();

            await _updateBudgetCommand.Execute(viewModel);
            return Ok();
        }

        [HttpPost("copy")]
        public async Task<IActionResult> CopyBudget()
        {
            await _copyBudgetCommand.Execute();
            return Ok();
        }
    }
}