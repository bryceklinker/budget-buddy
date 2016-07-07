namespace BudgetBuddy.Api.Budgets.ViewModels
{
    public class BudgetViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public BudgetCategoryViewModel[] Categories { get; set; }
    }
}
