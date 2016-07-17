using System;

namespace BudgetBuddy.Api.Budgets.Shared.ViewModels
{
    public class BudgetCategoryViewModel
    {
        public string Name { get; set; }

        public BudgetLineItemViewModel[] LineItems { get; set; }

        public BudgetCategoryViewModel()
        {
            LineItems = new BudgetLineItemViewModel[0];
        }
    }
}
