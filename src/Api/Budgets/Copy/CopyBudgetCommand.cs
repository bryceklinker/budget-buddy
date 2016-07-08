using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Shared.Model;
using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using BudgetBuddy.Api.General;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Hangfire.Common;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Copy
{
    public interface ICopyBudgetCommand
    {
        Task Execute();
    }

    [Transient(typeof(ICopyBudgetCommand))]
    public class CopyBudgetCommand : ICopyBudgetCommand
    {
        public static readonly Job Job = new Job(typeof(ICopyBudgetCommand), typeof(ICopyBudgetCommand).GetMethod("Execute"));
        public const string JobId = "budgets-copy-or-create-next";
        private readonly BudgetContext _budgetContext;
        private readonly IDateTimeService _dateTimeService;

        public CopyBudgetCommand(BudgetContext budgetContext, IDateTimeService dateTimeService)
        {
            _budgetContext = budgetContext;
            _dateTimeService = dateTimeService;
        }

        public async Task Execute()
        {
            var currentBudget = await _budgetContext.Budgets
                .OrderByDescending(b => b.StartDate)
                .FirstOrDefaultAsync();

            var newBudget = currentBudget != null ? Copy(currentBudget) : CreateBudget();
            _budgetContext.Add(newBudget);
            await _budgetContext.SaveChangesAsync();
        }

        private Budget CreateBudget()
        {
            return new Budget
            {
                StartDate = new DateTime(_dateTimeService.Now.Year, _dateTimeService.Now.Month, 1)
            };
        }

        private Budget Copy(Budget budget)
        {
            return new Budget
            {
                StartDate = budget.StartDate.AddMonths(1),
                LineItems = budget.LineItems?.Select(Copy).ToList()
            };
        }

        private BudgetLineItem Copy(BudgetLineItem budgetLineItem)
        {
            return new BudgetLineItem
            {
                Name = budgetLineItem.Name,
                Estimate = budgetLineItem.Estimate,
                Category = budgetLineItem.Category
            };
        }
    }
}