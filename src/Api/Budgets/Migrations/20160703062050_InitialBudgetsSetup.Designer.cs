using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BudgetBuddy.Api.Budgets.Model;

namespace Api.Budgets.Migrations
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20160703062050_InitialBudgetsSetup")]
    partial class InitialBudgetsSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Model.Entities.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Month");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Model.Entities.BudgetLineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Actual");

                    b.Property<Guid?>("BudgetId")
                        .IsRequired();

                    b.Property<Guid?>("CategoryId")
                        .IsRequired();

                    b.Property<decimal>("Estimate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BudgetLineItems");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Model.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BudgetBuddy.Api.Budgets.Model.Entities.BudgetLineItem", b =>
                {
                    b.HasOne("BudgetBuddy.Api.Budgets.Model.Entities.Budget", "Budget")
                        .WithMany("LineItems")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetBuddy.Api.Budgets.Model.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
