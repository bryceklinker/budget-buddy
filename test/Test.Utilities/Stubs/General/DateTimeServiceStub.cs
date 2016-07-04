using BudgetBuddy.Core.General;

namespace BudgetBuddy.Test.Utilities.Stubs.General
{
    public class DateTimeServiceStub : IDateTimeService
    {
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
    }
}
