using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Core.Budgets.Model.Entities
{
    public class Budget
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public virtual ICollection<BudgetLineItem> LineItems { get; set; }
    }
}
