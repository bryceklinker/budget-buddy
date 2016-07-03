using BudgetBuddy.Infrastructure.DependencyInjection;

namespace BudgetBuddy.Api.General
{
    public interface IDateTimeService
    {
        int CurrentMonth { get; }
        int CurrentYear { get; }
    }

    [Singleton(typeof(IDateTimeService))]
    public class DateTimeService : IDateTimeService
    {
        public int CurrentMonth { get; }
        public int CurrentYear { get; }
    }
}
