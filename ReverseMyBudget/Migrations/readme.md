// Run these scripts from the ReverseMyBudget project folder

// Initial creation
dotnet ef migrations add InitialCreate --context ReverseMyBudgetDbContext --output-dir Migrations/ReverseMyBudget
dotnet ef migrations add InitialCreate --context ApplicationUserDbContext --output-dir Migrations/ApplicationUserDbContext

// Update
dotnet ef migrations add NewCommit --context ReverseMyBudgetDbContext
dotnet ef migrations add NewCommit --context ApplicationUserDbContext

// Run migrations
dotnet ef database update --context ReverseMyBudgetDbContext
dotnet ef database update --context ApplicationUserDbContext

// Revert a migration - have to update DB first, then remove the migration
dotnet ef database update <Last known good migration> --context <the DbContext>
dotnet ef migrations remove
