using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Telemetry;
using BudgetBuddy.Api.Telemetry.Model.Entities;
using Microsoft.AspNetCore.Http;
using BudgetBuddy.Infrastructure.DependencyInjection;
using BudgetBuddy.Test.Utilities;
using BudgetBuddy.Test.Utilities.Stubs.General;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using Xunit;


namespace BudgetBuddy.Api.Test.Telemetry
{
    
    public class TelemetryServiceTest
    {
        private readonly InMemoryRepository<TelemetryException> _exceptionRepository;
        private readonly InMemoryRepository<TelemetryEvent> _eventRepository;
        private readonly InMemoryRepository<TelemetryTiming> _timingRepository;
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

            _exceptionRepository = new InMemoryRepository<TelemetryException>();
            _eventRepository = new InMemoryRepository<TelemetryEvent>();
            _timingRepository = new InMemoryRepository<TelemetryTiming>();
            _telemetryService = new TelemetryService(_exceptionRepository, _eventRepository, _timingRepository, _dateTimeServiceStub);
        }

        [Fact]
        public async Task TrackEvent_ShouldAddEvent()
        {
            await _telemetryService.TrackEvent(_httpContext);
            AssertEventEqual(_httpContext, _eventRepository.Entities.Single());
        }

        [Fact]
        public async Task TrackTiming_ShouldAddTiming()
        {
            await _telemetryService.TrackTiming("http://localhost/budgets/06/2016", TimeSpan.FromSeconds(4));

            var telemetryTiming = _timingRepository.Entities.Single();
            Assert.Equal(_dateTimeServiceStub.Now, telemetryTiming.Timestamp);
            Assert.Equal("http://localhost/budgets/06/2016", telemetryTiming.Url);
            Assert.Equal(TimeSpan.FromSeconds(4), telemetryTiming.Timespan);
        }

        [Fact]
        public async Task TrackException_ShouldAddException()
        {
            var exception = new Exception("Some message");
            exception.Data["bob"] = "John";
            exception.Data["Jill"] = "Jack";

            await _telemetryService.TrackException(_httpContext, exception);
            var telemetryException = _exceptionRepository.Entities.Single();
            Assert.Equal(_dateTimeServiceStub.Now, telemetryException.Timestamp);
            Assert.Equal(_httpContext.Request.GetDisplayUrl(), telemetryException.Url);
            Assert.Equal(_httpContext.Request.Method, telemetryException.Method);
            Assert.Equal("Some message", telemetryException.Message);
            Assert.Equal("{\"bob\":\"John\",\"Jill\":\"Jack\"}", telemetryException.Data);
            Assert.Equal(exception.StackTrace, telemetryException.StackTrace);
        }

        [Fact]
        public void TelemetryService_ShouldBeTransient()
        {
            var transient = _telemetryService.GetAttribute<TransientAttribute>();
            Assert.Equal(typeof(ITelemetryService), transient.InterfaceType);
        }

        private void AssertEventEqual(HttpContext httpContext, TelemetryEvent telemetryEvent)
        {
            Assert.Equal(httpContext.Request.GetDisplayUrl(), telemetryEvent.Url);
            Assert.Equal(httpContext.Request.Method, telemetryEvent.HttpMethod);
            Assert.Equal(_dateTimeServiceStub.Now, telemetryEvent.Timestamp);
        }
    }
}
