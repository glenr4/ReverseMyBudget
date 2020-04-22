using Microsoft.EntityFrameworkCore.Migrations;
using ReverseMyBudget.Persistence.Sql;

namespace ReverseMyBudget.Migrations.ReverseMyBudget
{
    public partial class AddDistinctToTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var storedProcedure = new StoredProcedureAddDistinctToTransactions();

            storedProcedure.Create(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var storedProcedure = new StoredProcedureAddDistinctToTransactions();

            storedProcedure.Drop(migrationBuilder);
        }
    }
}