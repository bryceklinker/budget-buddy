using BudgetBuddy.Api.Telemetry.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Telemetry.Model
{
    public class TelemetryContext : DbContext
    {
        public DbSet<TelemetryEvent> TelemetryEvents { get; set; }
        public DbSet<TelemetryTiming> TelemetryTimings { get; set; }

        public TelemetryContext(DbContextOptions<TelemetryContext> options)
            : base(options)
        {
            
        }
    }
}
