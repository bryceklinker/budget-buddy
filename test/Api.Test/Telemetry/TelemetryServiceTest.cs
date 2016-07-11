using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Telemetry;
using BudgetBuddy.Api.Telemetry.Model;
using BudgetBuddy.Api.Telemetry.Model.Entities;
using BudgetBuddy.Test.Utilities.Factories;
using Microsoft.AspNetCore.Http;
using Xunit;
using System.Linq;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Microsoft.AspNetCore.Http.Extensions;

namespace BudgetBuddy.Api.Test.Telemetry
{
    public class TelemetryServiceTest
    {
        private readonly TelemetryContext _telemetryContext;
        private readonly DateTimeServiceStub _dateTimeServiceStub;
        private readonly HttpContext _httpContext;
        private readonly TelemetryService _telemetryService;

        public TelemetryServiceTest()
        {
            _dateTimeServiceStub = new DateTimeServiceStub
            {
                Now = DateTime.Now
            };

            _httpContext = new DefaultHttpContext
            {
                Request =
                {
                    Method = "GET",
                    Protocol = "http",
                    Host = new HostString("localhost", 80),
                    Path = PathString.FromUriComponent("/budgets/06/2016")
                }
            };
            _telemetryContext = DbContextFactory.Create<TelemetryContext>();
            _telemetryService = new TelemetryService(_telemetryContext, _dateTimeServiceStub);
        }

        [Fact]
        public async Task TrackEvent_ShouldAddEvent()
        {
            await _telemetryService.TrackEvent(_httpContext);
            AssertEventEqual(_httpContext, _telemetryContext.TelemetryEvents.SingleOrDefault());
        }

        [Fact]
        public async Task TrackTiming_ShouldAddTiming()
        {
            await _telemetryService.TrackTiming("http://localhost/budgets/06/2016", TimeSpan.FromSeconds(4));

            var telemetryTiming = _telemetryContext.TelemetryTimings.SingleOrDefault();
            Assert.Equal("http://localhost/budgets/06/2016", telemetryTiming.Url);
            Assert.Equal(TimeSpan.FromSeconds(4), telemetryTiming.Timespan);
        }

        private void AssertEventEqual(HttpContext httpContext, TelemetryEvent telemetryEvent)
        {
            Assert.Equal(httpContext.Request.GetDisplayUrl(), telemetryEvent.Url);
            Assert.Equal(httpContext.Request.Method, telemetryEvent.HttpMethod);
            Assert.Equal(_dateTimeServiceStub.Now, telemetryEvent.Timestamp);
        }
    }
}
