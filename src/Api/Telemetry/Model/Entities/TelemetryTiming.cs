using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Api.Telemetry.Model.Entities
{
    public class TelemetryTiming
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public TimeSpan Timespan { get; set; }
    }
}
