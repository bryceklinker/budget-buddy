using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.General;
using BudgetBuddy.Api.General.Storage;
using BudgetBuddy.Api.Telemetry.Model.Entities;
using BudgetBuddy.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace BudgetBuddy.Api.Telemetry
{
    public interface ITelemetryService
    {
        Task TrackEvent(HttpContext httpContext);
        Task TrackTiming(string url, TimeSpan timespan);
        Task TrackException(HttpContext httpContext, Exception exception);
    }

    [Transient(typeof(ITelemetryService))]
    public class TelemetryService : ITelemetryService
    {
        private readonly IRepository<TelemetryException> _exceptionRepository;
        private readonly IRepository<TelemetryEvent> _eventRepository;
        private readonly IRepository<TelemetryTiming> _timingRepository;
        private readonly IDateTimeService _dateTimeService;

        public TelemetryService(IRepository<TelemetryException> exceptionRepository, IRepository<TelemetryEvent> eventRepository, IRepository<TelemetryTiming> timingRepository, IDateTimeService dateTimeService)
        {
            _exceptionRepository = exceptionRepository;
            _dateTimeService = dateTimeService;
            _eventRepository = eventRepository;
            _timingRepository = timingRepository;
        }

        public async Task TrackEvent(HttpContext httpContext)
        {
            var telementryEvent = CreateEvent(httpContext);
            await _eventRepository.Insert(telementryEvent);
        }

        public async Task TrackTiming(string url, TimeSpan timespan)
        {
            var timing = CreateTimeing(url, timespan);
            await _timingRepository.Insert(timing);
        }

        public async Task TrackException(HttpContext httpContext, Exception exception)
        {
            var telemetryException = CreateException(httpContext, exception);
            await _exceptionRepository.Insert(telemetryException);
        }

        private TelemetryException CreateException(HttpContext httpContext, Exception exception)
        {
            return new TelemetryException
            {
                Timestamp = _dateTimeService.Now,
                Data = JsonConvert.SerializeObject(exception.Data),
                Method = httpContext.Request.Method,
                Url = httpContext.Request.GetDisplayUrl(),
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };
        }

        private TelemetryEvent CreateEvent(HttpContext context)
        {
            return new TelemetryEvent
            {
                Timestamp = _dateTimeService.Now,
                HttpMethod = context.Request.Method,
                Url = context.Request.GetDisplayUrl()
            };
        }

        private TelemetryTiming CreateTimeing(string url, TimeSpan timespan)
        {
            return new TelemetryTiming
            {
                Timestamp = _dateTimeService.Now,
                Url = url,
                Timespan = timespan
            };
        }
    }
}