using System;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Api.Telemetry.Model.Entities
{
    public class TelemetryException : IDocument
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Data { get; set; }
    }
}
