using System.Threading.Tasks;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Get
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
            return _budgetContext.Budgets
                .Where(b => b.StartDate.Month == month)
                .Where(b => b.StartDate.Year == year)
                .Select(b => new BudgetViewModel
                {
                    Income = b.Income,
                    StartDate = b.StartDate,
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
                .SingleOrDefaultAsync();
        }
    }
}
