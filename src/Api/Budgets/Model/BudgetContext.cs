using BudgetBuddy.Api.Budgets.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Budgets.Model
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
    }
}
