using System;
using BudgetBuddy.Api.General;

namespace BudgetBuddy.Test.Utilities.Stubs.General
{
    public class DateTimeServiceStub : IDateTimeService
    {
        public DateTime Now { get; set; }
    }
}
