using System;
using System.Threading.Tasks;
using System.Linq;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.Budgets.Get
{
    public interface IGetBudgetQuery
    {
        Task<BudgetViewModel> Execute(int year, int month);
    }

    [Transient(typeof(IGetBudgetQuery))]
    public class GetBudgetQuery : IGetBudgetQuery
    {
        private readonly IRepository<Budget> _budgetRepository;

        public GetBudgetQuery(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<BudgetViewModel> Execute(int year, int month)
        {
            var budgets = await _budgetRepository.Query(b => b.StartDate == new DateTime(year, month, 1));
            return budgets
                .Select(b => new BudgetViewModel
                {
                    Income = b.Income,
                    StartDate = b.StartDate,
                    Categories = b.Categories.Select(c => new BudgetCategoryViewModel
                    {
                        Name = c.Name,
                        LineItems = c.BudgetLineItems.Select(i => new BudgetLineItemViewModel
                        {
                            Name = i.Name,
                            Actual = i.Actual,
                            Estimate = i.Estimate
                        }).ToArray()
                    }).ToArray()
                })
                .SingleOrDefault();
        }
    }
}
