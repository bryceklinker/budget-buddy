using System;
using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.General
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }

    [Singleton(typeof(IDateTimeService))]
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
