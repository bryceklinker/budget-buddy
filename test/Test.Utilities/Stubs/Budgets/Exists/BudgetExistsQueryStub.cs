using System.Threading.Tasks;
using BudgetBuddy.Core.Budgets.Exists;

namespace BudgetBuddy.Test.Utilities.Stubs.Budgets.Exists
{
    public class BudgetExistsQueryStub : IBudgetExistsQuery
    {
        public bool Exists { get; set; }
        public int Month { get; private set; }
        public int Year { get; private set; }


        public Task<bool> Execute(int month, int year)
        {
            Month = month;
            Year = year;
            return Task.FromResult(Exists);
        }
    }
}
