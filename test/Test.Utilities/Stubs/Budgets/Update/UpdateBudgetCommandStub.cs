using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Shared.ViewModels;
using BudgetBuddy.Api.Budgets.Update;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Update
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
