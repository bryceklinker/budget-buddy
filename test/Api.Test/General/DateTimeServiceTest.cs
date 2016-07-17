using System;
using BudgetBuddy.Api.General;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using Xunit;


namespace BudgetBuddy.Api.Test.General
{
    
    public class DateTimeServiceTest
    {
        private readonly DateTimeService _dateTimeService;

        public DateTimeServiceTest()
        {
            _dateTimeService = new DateTimeService();
        }

        [Fact]
        public void CurrentMonth_ShouldGetCurrentDate()
        {
            Assert.Equal(DateTime.Now.Date, _dateTimeService.Now.Date);
        }

        [Fact]
        public void DateTimeService_ShouldBeSingleton()
        {
            var singleton = _dateTimeService.GetAttribute<SingletonAttribute>();
            Assert.Equal(typeof(IDateTimeService), singleton.InterfaceType);
        }
    }
}
