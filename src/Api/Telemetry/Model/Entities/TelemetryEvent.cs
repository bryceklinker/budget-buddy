using System;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Api.Telemetry.Model.Entities
{
    public class TelemetryEvent : IDocument
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Url { get; set; }

        public string Feature { get; set; }

        public string HttpMethod { get; set; }
    }
}
