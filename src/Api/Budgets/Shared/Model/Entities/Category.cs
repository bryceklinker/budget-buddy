using System.Collections.Generic;

namespace BudgetBuddy.Api.Budgets.Shared.Model.Entities
{
    public class Category
    {
        public string Name { get; set; }

        public List<BudgetLineItem> BudgetLineItems { get; set; }

        public Category()
        {
            BudgetLineItems = new List<BudgetLineItem>();
        }
    }
}
