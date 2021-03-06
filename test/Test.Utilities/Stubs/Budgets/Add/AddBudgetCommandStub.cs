﻿using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Add;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Add
{
    public class AddBudgetCommandStub : IAddBudgetCommand
    {
        public BudgetViewModel AddedBudget { get; private set; }
        public Guid NewId { get; private set; }

        public Task<Guid> Execute(BudgetViewModel budgetViewModel)
        {
            NewId = Guid.NewGuid();
            AddedBudget = budgetViewModel;
            return Task.FromResult(NewId);
        }
    }
}
