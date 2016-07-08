using System;

namespace BudgetBuddy.Api.Budgets.Shared.ViewModels
{
    public class BudgetCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public BudgetLineItemViewModel[] LineItems { get; set; }
    }
}
