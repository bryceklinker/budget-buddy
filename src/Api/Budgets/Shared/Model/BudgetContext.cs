using BudgetBuddy.Api.Budgets.Shared.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Shared.Model
{
    public class BudgetContext : DbContext
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetLineItem> BudgetLineItems { get; set; }

        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>()
                .HasIndex(b => new {b.StartDate})
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<BudgetLineItem>()
                .HasIndex(l => new {l.CategoryId, l.BudgetId, l.Name})
                .IsUnique();

            modelBuilder.Entity<BudgetLineItem>()
                .HasOne(b => b.Budget)
                .WithMany(b => b.LineItems)
                .HasForeignKey(b => b.BudgetId)
                .IsRequired();

            modelBuilder.Entity<BudgetLineItem>()
                .HasOne(b => b.Category)
                .WithMany(b => b.BudgetLineItems)
                .HasForeignKey(b => b.CategoryId)
                .IsRequired();
        }
    }
}
