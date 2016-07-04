using System;
using BudgetBuddy.Core.General;
using Xunit;

namespace BudgetBuddy.Core.Test.General
{
    public class DateTimeServiceTest
    {
        private readonly DateTimeService _dateTimeService;

        public DateTimeServiceTest()
        {
            _dateTimeService = new DateTimeService();
        }

        [Fact]
        public void CurrentMonth_ShouldGetCurrentMonth()
        {
            Assert.Equal(DateTime.Now.Month, _dateTimeService.CurrentMonth);
        }

        [Fact]
        public void CurrentYear_ShouldGetCurrentYear()
        {
            Assert.Equal(DateTime.Now.Year, _dateTimeService.CurrentYear);
        }
    }
}
