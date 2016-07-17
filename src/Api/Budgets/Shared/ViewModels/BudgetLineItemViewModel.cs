using System;

namespace BudgetBuddy.Api.Budgets.Shared.ViewModels
{
    public class BudgetLineItemViewModel
    {
        public string Name { get; set; }
        public decimal Estimate { get; set; }
        public decimal Actual { get; set; }
    }
}
