using System;
using System.Linq;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy.ViewModels;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.Budgets.Copy
{
    public interface ICopyBudgetCommand
    {
        Task Execute(CopyBudgetViewModel viewModel);
    }

    [Transient(typeof(ICopyBudgetCommand))]
    public class CopyBudgetCommand : ICopyBudgetCommand
    {
        public const string JobId = "budgets-copy-or-create-next";
        private readonly IRepository<Budget> _budgetRepository;

        public CopyBudgetCommand(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task Execute(CopyBudgetViewModel viewModel)
        {
            var fromBudget = await GetBudget(viewModel.FromYear, viewModel.FromMonth);
            var toBudget = await GetBudget(viewModel.ToYear, viewModel.ToMonth);
            if (toBudget != null)
                throw new InvalidOperationException();

            toBudget = Copy(fromBudget, viewModel.ToYear, viewModel.ToMonth);
            await _budgetRepository.Insert(toBudget);
        }

        private async Task<Budget> GetBudget(int year, int month)
        {
            var budgets = await _budgetRepository.GetAll();
            return budgets
                .SingleOrDefault(b => b.StartDate == new DateTime(year, month, 1));
        }

        private static Budget Copy(Budget budget, int toYear, int toMonth)
        {
            return new Budget
            {
                StartDate = new DateTime(toYear, toMonth, 1),
                Categories = budget.Categories.Select(Copy).ToList()
            };
        }

        private static Category Copy(Category category)
        {
            return new Category
            {
                Name = category.Name,
                BudgetLineItems = category.BudgetLineItems.Select(Copy).ToList()
            };
        }

        private static BudgetLineItem Copy(BudgetLineItem budgetLineItem)
        {
            return new BudgetLineItem
            {
                Name = budgetLineItem.Name,
                Estimate = budgetLineItem.Estimate
            };
        }
    }
}