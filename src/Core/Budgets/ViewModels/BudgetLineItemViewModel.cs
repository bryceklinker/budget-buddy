﻿using System;

namespace BudgetBuddy.Core.Budgets.ViewModels
{
    public class BudgetLineItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Estimate { get; set; }
        public decimal Actual { get; set; }
    }
}