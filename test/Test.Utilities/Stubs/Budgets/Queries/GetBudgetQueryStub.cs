using System.Threading.Tasks;
using BudgetBuddy.Api.Budgets.Queries.GetBudget;
using BudgetBuddy.Api.Budgets.Queries.GetBudget.ViewModels;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Queries
{
    public class GetBudgetQueryStub : IGetBudgetQuery
    {
        public int Month { get; private set; }
        public int Year { get; private set; }
        public BudgetViewModel Result { get; set; }

        public Task<BudgetViewModel> Execute(int month, int year)
        {
            Month = month;
            Year = year;
            return Task.FromResult(Result);
        }
    }
}
