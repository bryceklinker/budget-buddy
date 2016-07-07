using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Api.Budgets.Model.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; }
    }
}
