using System;

namespace BudgetBuddy.Core.Budgets.ViewModels
{
    public class BudgetCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public BudgetLineItemViewModel[] LineItems { get; set; }
    }
}
