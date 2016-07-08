using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BudgetBuddy.Api.Budgets.Shared.Model;

namespace Api.Budgets.Shared.Model.Migrations
{
    [DbContext(typeof(BudgetContext))]
    partial class BudgetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Shared.Model.Entities.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("StartDate")
                        .IsUnique();

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Shared.Model.Entities.BudgetLineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Actual");

                    b.Property<Guid>("BudgetId");

                    b.Property<Guid>("CategoryId");

                    b.Property<decimal>("Estimate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CategoryId", "BudgetId", "Name")
                        .IsUnique();

                    b.ToTable("BudgetLineItems");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Shared.Model.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Shared.Model.Entities.BudgetLineItem", b =>
                {
                    b.HasOne("BudgetBuddy.Api.Budgets.Shared.Model.Entities.Budget", "Budget")
                        .WithMany("LineItems")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetBuddy.Api.Budgets.Shared.Model.Entities.Category", "Category")
                        .WithMany("BudgetLineItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
