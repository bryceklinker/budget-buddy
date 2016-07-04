using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.UpdateBudget;
using BudgetBuddy.Core.Budgets.ViewModels;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.UpdateBudget
{
    public class UpdateBudgetCommandStub : IUpdateBudgetCommand
    {
        public BudgetViewModel BudgetViewModel { get; private set; }

        public Task Execute(BudgetViewModel budgetViewModel)
        {
            BudgetViewModel = budgetViewModel;
            return Task.CompletedTask;
        }
    }
}
