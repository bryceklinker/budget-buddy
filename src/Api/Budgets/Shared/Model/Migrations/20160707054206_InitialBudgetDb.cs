using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Budgets.Shared.Model.Migrations
{
    public partial class InitialBudgetDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Actual = table.Column<decimal>(nullable: false),
                    BudgetId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Estimate = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLineItems_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetLineItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_StartDate",
                table: "Budgets",
                column: "StartDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLineItems_BudgetId",
                table: "BudgetLineItems",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLineItems_CategoryId",
                table: "BudgetLineItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLineItems_CategoryId_BudgetId_Name",
                table: "BudgetLineItems",
                columns: new[] { "CategoryId", "BudgetId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetLineItems");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
