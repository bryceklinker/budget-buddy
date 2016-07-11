using System;

namespace BudgetBuddy.Api.Budgets.Shared.ViewModels
{
    public class BudgetViewModel
    {
        public decimal Income { get; set; }
        public DateTime StartDate { get; set; }
        public int Month => StartDate.Month;
        public int Year => StartDate.Year;

        public BudgetCategoryViewModel[] Categories { get; set; }
    }
}
