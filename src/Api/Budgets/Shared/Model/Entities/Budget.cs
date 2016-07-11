using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Api.Budgets.Shared.Model.Entities
{
    public class Budget
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public decimal Income { get; set; }

        public virtual ICollection<BudgetLineItem> LineItems { get; set; }

        public Budget()
        {
            LineItems = new List<BudgetLineItem>();
        }
    }
}
