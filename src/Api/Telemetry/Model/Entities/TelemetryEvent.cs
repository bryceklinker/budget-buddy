using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Api.Telemetry.Model.Entities
{
    public class TelemetryEvent
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [MaxLength(int.MaxValue)]
        public string Url { get; set; }

        [MaxLength(255)]
        public string Feature { get; set; }

        [MaxLength(255)]
        public string HttpMethod { get; set; }
    }
}
