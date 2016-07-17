using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Telemetry;
using Microsoft.AspNetCore.Http;

namespace BudgetBuddy.Test.Utilities.Stubs.Telemetry
{
    public class TelemetryServiceStub : ITelemetryService
    {
        public HttpContext HttpContext { get; private set; }

        public string TimingUrl { get; private set; }
        public TimeSpan TimingTimespan { get; private set; }

        public HttpContext ExceptionContext { get; private set; }
        public Exception Exception { get; private set; }

        public Task TrackEvent(HttpContext httpContext)
        {
            HttpContext = httpContext;
            return Task.CompletedTask;;
        }

        public Task TrackTiming(string url, TimeSpan timespan)
        {
            TimingUrl = url;
            TimingTimespan = timespan;
            return Task.CompletedTask;;
        }

        public Task TrackException(HttpContext httpContext, Exception exception)
        {
            Exception = exception;
            ExceptionContext = httpContext;
            return Task.CompletedTask;;
        }
    }
}
