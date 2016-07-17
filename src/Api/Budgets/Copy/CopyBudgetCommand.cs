using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.General;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Hangfire.Common;

namespace BudgetBuddy.Api.Budgets.Copy
{
    public interface ICopyBudgetCommand
    {
        Task Execute();
    }

    [Transient(typeof(ICopyBudgetCommand))]
    public class CopyBudgetCommand : ICopyBudgetCommand
    {
        public const string JobId = "budgets-copy-or-create-next";
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IDateTimeService _dateTimeService;

        public static Job CreateCopyJob()
        {
            return new Job(typeof(ICopyBudgetCommand), typeof(ICopyBudgetCommand).GetMethod("Execute"));
        }

        public CopyBudgetCommand(IRepository<Budget> budgetRepository, IDateTimeService dateTimeService)
        {
            _budgetRepository = budgetRepository;
            _dateTimeService = dateTimeService;
        }

        public async Task Execute()
        {
            var currentBudget = await GetCurrentBudget();
            if (currentBudget == null)
                await CreateCurrentBudget();
            else
                await CopyCurrentBudgetForNextMonth(currentBudget);
        }

        private async Task<Budget> GetCurrentBudget()
        {
            var budgets = await _budgetRepository.GetAll();
            return budgets.OrderByDescending(b => b.StartDate)
                .FirstOrDefault();
        }

        private async Task CreateCurrentBudget()
        {
            var currentBudget = new Budget
            {
                StartDate = new DateTime(_dateTimeService.Now.Year, _dateTimeService.Now.Month, 1)
            };
            await _budgetRepository.Insert(currentBudget);
        }

        private async Task CopyCurrentBudgetForNextMonth(Budget currentBudget)
        {
            var nextMonthsBudget = await GetNextMonthsBudget();
            if (nextMonthsBudget != null)
                return;

            nextMonthsBudget = Copy(currentBudget);
            await _budgetRepository.Insert(nextMonthsBudget);
        }

        private async Task<Budget> GetNextMonthsBudget()
        {
            var nextStartDate = _dateTimeService.Now.AddMonths(1);
            var budgets = await _budgetRepository.GetAll();
            return budgets.FirstOrDefault(b => b.StartDate == new DateTime(nextStartDate.Year, nextStartDate.Month, 1));
        }

        private static Budget Copy(Budget budget)
        {
            return new Budget
            {
                StartDate = budget.StartDate.AddMonths(1),
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