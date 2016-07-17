using System;
using System.Collections.Generic;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Api.Budgets.Shared.Model.Entities
{
    public class Budget : IDocument
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public decimal Income { get; set; }

        public List<Category> Categories { get; set; }

        public Budget()
        {
            Categories = new List<Category>();
        }
    }
}
