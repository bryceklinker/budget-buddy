namespace BudgetBuddy.Core.Budgets.ViewModels
{
    public class BudgetViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public BudgetCategoryViewModel[] Categories { get; set; }
    }
}
