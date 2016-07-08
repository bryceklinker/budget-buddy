using System;

namespace BudgetBuddy.Api.Budgets.Shared.ViewModels
{
    public class BudgetViewModel
    {
        public DateTime StartDate { get; set; }
        public int Month => StartDate.Month;
        public int Year => StartDate.Year;

        public BudgetCategoryViewModel[] Categories { get; set; }
    }
}
