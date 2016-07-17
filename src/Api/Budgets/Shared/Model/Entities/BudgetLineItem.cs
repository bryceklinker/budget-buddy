namespace BudgetBuddy.Api.Budgets.Shared.Model.Entities
{
    public class BudgetLineItem
    {
        public string Name { get; set; }
        public decimal Estimate { get; set; }
        public decimal Actual { get; set; }
    }
}
