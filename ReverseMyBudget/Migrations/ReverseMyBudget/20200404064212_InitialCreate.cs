using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReverseMyBudget.Migrations.ReverseMyBudget
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
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
                    ImportOrder = table.Column<int>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: true),
                    IsDuplicate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
