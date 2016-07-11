using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.General;
using BudgetBuddy.Api.Telemetry.Model;
using BudgetBuddy.Api.Telemetry.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace BudgetBuddy.Api.Telemetry
{
    public interface ITelemetryService
    {
        Task TrackEvent(HttpContext httpContext);
        Task TrackTiming(string url, TimeSpan timespan);
    }

    public class TelemetryService : ITelemetryService
    {
        private readonly TelemetryContext _telemetryContext;
        private readonly IDateTimeService _dateTimeService;

        public TelemetryService(TelemetryContext telemetryContext, IDateTimeService dateTimeService)
        {
            _telemetryContext = telemetryContext;
            _dateTimeService = dateTimeService;
        }

        public async Task TrackEvent(HttpContext httpContext)
        {
            var telementryEvent = CreateEvent(httpContext);
            _telemetryContext.Add(telementryEvent);
            await _telemetryContext.SaveChangesAsync();
        }

        public async Task TrackTiming(string url, TimeSpan timespan)
        {
            var timing = CreateTimeing(url, timespan);
            _telemetryContext.Add(timing);
            await _telemetryContext.SaveChangesAsync();
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
                Url = url,
                Timespan = timespan
            };
        }
    }
}