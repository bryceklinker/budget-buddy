using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Model;
using BudgetBuddy.Api.Budgets.Queries.GetBudget.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.Budgets.Queries.GetBudget
{
    public interface IGetBudgetQuery
    {
        Task<BudgetViewModel> Execute(int month, int year);
    }

    [Transient(typeof(IGetBudgetQuery))]
    public class GetBudgetQuery : IGetBudgetQuery
    {
        private readonly BudgetContext _budgetContext;

        public GetBudgetQuery(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        public Task<BudgetViewModel> Execute(int month, int year)
        {
            var viewModel = _budgetContext.Budgets
                .Where(b => b.Month == month)
                .Where(b => b.Year == year)
                .Select(b => new BudgetViewModel
                {
                    Month = b.Month,
                    Year = b.Year,
                    Categories = b.LineItems
                        .GroupBy(l => new { l.Category.Id, l.Category.Name })
                        .Select(c => new BudgetCategoryViewModel
                        {
                            Id = c.Key.Id,
                            Name = c.Key.Name,
                            LineItems = c.Select(l => new BudgetLineItemViewModel
                            {
                                Name = l.Name,
                                Id = l.Id,
                                Actual = l.Actual,
                                Estimate = l.Estimate
                            })
                            .ToArray()
                        })
                        .ToArray()
                })
                .SingleOrDefault();
            return Task.FromResult(viewModel);
        }
    }
}
