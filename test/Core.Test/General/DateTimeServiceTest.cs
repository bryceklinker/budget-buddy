using System;
using BudgetBuddy.Core.General;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
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

        [Fact]
        public void DateTimeService_ShouldBeSingleton()
        {
            var singleton = _dateTimeService.GetAttribute<SingletonAttribute>();
            Assert.Equal(typeof(IDateTimeService), singleton.InterfaceType);
        }
    }
}
