using BudgetBuddy.Core.Budgets.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Core.Budgets.Model
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
                .HasAlternateKey(b => new {b.Year, b.Month})
                .HasName("AK_Budget_Month_Year");

            modelBuilder.Entity<BudgetLineItem>()
                .HasOne(l => l.Category)
                .WithMany(c => c.BudgetLineItems)
                .HasForeignKey(l => l.CategoryId)
                .IsRequired();

            modelBuilder.Entity<BudgetLineItem>()
                .HasOne(l => l.Budget)
                .WithMany(b => b.LineItems)
                .HasForeignKey(l => l.BudgetId)
                .IsRequired();

            modelBuilder.Entity<BudgetLineItem>()
                .HasAlternateKey(l => new {l.CategoryId, l.BudgetId, l.Name})
                .HasName("AK_BudgetLineItem_Category_Budget_Name");
        }
    }
}
