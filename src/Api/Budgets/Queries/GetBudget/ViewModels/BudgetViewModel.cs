using System.Collections.Generic;

namespace BudgetBuddy.Api.Budgets.Queries.GetBudget.ViewModels
{
    public class BudgetViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public BudgetCategoryViewModel[] Categories { get; set; }
    }
}
