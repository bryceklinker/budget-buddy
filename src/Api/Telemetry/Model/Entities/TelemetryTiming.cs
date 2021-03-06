﻿using System;
using System.ComponentModel.DataAnnotations;
using BudgetBuddy.Api.General.Storage;

namespace BudgetBuddy.Api.Telemetry.Model.Entities
{
    public class TelemetryTiming : IDocument
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Url { get; set; }
        public TimeSpan Timespan { get; set; }
    }
}
