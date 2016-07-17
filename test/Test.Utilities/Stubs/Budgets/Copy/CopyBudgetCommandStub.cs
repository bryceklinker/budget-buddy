using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Copy;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Copy
{
    public class CopyBudgetCommandStub : ICopyBudgetCommand
    {
        public bool IsExecuted { get; private set; }

        public Task Execute()
        {
            IsExecuted = true;
            return Task.CompletedTask;
        }
    }
}
