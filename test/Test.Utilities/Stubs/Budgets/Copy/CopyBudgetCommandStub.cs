using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy;
using BudgetBuddy.Api.Budgets.Copy.ViewModels;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Copy
{
    public class CopyBudgetCommandStub : ICopyBudgetCommand
    {
        public CopyBudgetViewModel ViewModel { get; private set; }

        public Task Execute(CopyBudgetViewModel viewModel)
        {
            ViewModel = viewModel;
            return Task.CompletedTask;
        }
    }
}
