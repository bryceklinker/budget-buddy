using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace BudgetBuddy.Api.Telemetry
{
    public class TelemetryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITelemetryService _telemetryService;

        public TelemetryMiddleware(RequestDelegate next, ITelemetryService telemetryService)
        {
            _next = next;
            _telemetryService = telemetryService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _telemetryService.TrackEvent(httpContext);

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                await _next.Invoke(httpContext);
                stopWatch.Stop();

                await _telemetryService.TrackTiming(httpContext.Request.GetDisplayUrl(), stopWatch.Elapsed);
            }
            catch (Exception ex)
            {
                await _telemetryService.TrackException(httpContext, ex);
                throw;
            }
        }
    }
}