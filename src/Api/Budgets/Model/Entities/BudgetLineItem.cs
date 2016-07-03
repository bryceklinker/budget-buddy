using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Api.Budgets.Model.Entities
{
    public class BudgetLineItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Estimate { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Actual { get; set; }

        [Required]
        public virtual Budget Budget { get; set; }

        [Required]
        public virtual Category Category { get; set; }
    }
}
