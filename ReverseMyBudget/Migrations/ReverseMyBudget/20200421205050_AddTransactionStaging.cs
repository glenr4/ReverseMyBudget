using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReverseMyBudget.Migrations.ReverseMyBudget
{
    public partial class AddTransactionStaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transaction_UserId_DateLocal_Amount_Description",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionStaging",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DateLocal = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: false),
                    ImportOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStaging", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStaging");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId_DateLocal_Amount_Description",
                table: "Transaction",
                columns: new[] { "UserId", "DateLocal", "Amount", "Description" },
                unique: true,
                filter: "[Description] IS NOT NULL");
        }
    }
}
