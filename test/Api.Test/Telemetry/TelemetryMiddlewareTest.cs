using System;
using System.Threading.Tasks;
using BudgetBuddy.Api.Telemetry;
using BudgetBuddy.Test.Utilities.Stubs.Telemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Xunit;


namespace BudgetBuddy.Api.Test.Telemetry
{
    
    public class TelemetryMiddlewareTest
    {
        private readonly TelemetryServiceStub _telemetryServiceStub;
        private readonly RequestDelegate _next;
        private readonly HttpContext _httpContext;
        private readonly TelemetryMiddleware _telemetryMiddleware;
        private bool _throwException;

        public TelemetryMiddlewareTest()
        {
            _telemetryServiceStub = new TelemetryServiceStub();
            _next = Next;
            _httpContext = new DefaultHttpContext
            {
                Request =
                {
                    Host = new HostString("http://localhost", 8080),
                    Path = PathString.FromUriComponent("/budgets")
                }
            };
            _telemetryMiddleware = new TelemetryMiddleware(_next, _telemetryServiceStub);
        }

        [Fact]
        public async Task Invoke_ShouldTrackEvent()
        {
            await _telemetryMiddleware.Invoke(_httpContext);
            Assert.Same(_httpContext, _telemetryServiceStub.HttpContext);
        }

        [Fact]
        public async Task Invoke_ShouldTrackTiming()
        {
            await _telemetryMiddleware.Invoke(_httpContext);
            Assert.Equal(_httpContext.Request.GetDisplayUrl(), _telemetryServiceStub.TimingUrl);
            Assert.NotEqual(TimeSpan.Zero, _telemetryServiceStub.TimingTimespan);
        }

        [Fact]
        public async Task Invoke_ShouldTrackExceptions()
        {
            _throwException = true;
            Exception thrownException = null;

            try
            {
                await _telemetryMiddleware.Invoke(_httpContext);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
            
            Assert.Same(thrownException, _telemetryServiceStub.Exception);
            Assert.Same(_httpContext, _telemetryServiceStub.ExceptionContext);
        }

        private Task Next(HttpContext context)
        {
            if (_throwException)
                throw new Exception();

            return Task.CompletedTask;;
        }
    }
}
