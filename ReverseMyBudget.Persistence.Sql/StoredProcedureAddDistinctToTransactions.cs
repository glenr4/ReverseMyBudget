using Microsoft.EntityFrameworkCore.Migrations;

namespace ReverseMyBudget.Persistence.Sql
{
    public class StoredProcedureAddDistinctToTransactions
    {
        public static string Name { get; set; } = "[dbo].[sp_AddDistinctToTransactions]";

        public void Create(MigrationBuilder migrationBuilder)
        {
            var sp = $@"-- Inserts all new records from TransactionStaging to Transaction for a User
-- ie if it already exists in Transaction table then it will be ignored
-- Used to maintain the data integrity of the Transaction table

CREATE PROCEDURE {Name}
	@UserId uniqueidentifier,
	@RowCount int Output	-- returns the number of rows inserted
AS
BEGIN

INSERT INTO [dbo].[Transaction]
           ([Id]
           ,[UserId]
		   ,[DateLocal]
           ,[Amount]
           ,[Type]
           ,[Description]
           ,[Balance]
		   ,[AccountId]
		   ,[ImportOrder]
           ,[IsDuplicate])
(
	select [Id]
            ,[UserId]
		   ,[DateLocal]
           ,[Amount]
           ,[Type]
           ,[Description]
           ,[Balance]
		   ,[AccountId]
		   ,[ImportOrder]
           ,0
	from [dbo].[TransactionStaging] AS ts
	WHERE UserId = @UserId
		AND NOT EXISTS
		(
			 Select 1
			 From [Transaction] AS t
			 Where ts.DateLocal = t.DateLocal
				 AND ts.Amount = t.Amount
				 AND ts.Description = t.Description
				 AND ts.UserId = t.UserId
		)
);

-- Return INSERT (only) row count, must be done before DELETE
Select @RowCount = @@ROWCOUNT;

-- Clear the staging table for the User
DELETE FROM [dbo].[TransactionStaging]
WHERE UserId = @UserId;

END

GO

";

            migrationBuilder.Sql(sp);
        }

        public void Drop(MigrationBuilder migrationBuilder)
        {
            var sp = $@"DROP PROCEDURE {Name};
GO";

            migrationBuilder.Sql(sp);
        }
    }
}