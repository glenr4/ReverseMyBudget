// Initial creation
dotnet ef migrations add InitialCreate --context ReverseMyBudgetDbContext --output-dir Migrations/ReverseMyBudget
dotnet ef migrations add InitialCreate --context ApplicationUserDbContext --output-dir Migrations/ApplicationUserDbContext

// Update
Same as above but don't need to specify the output directory

// Run migrations
dotnet ef database update --context <DbContext Name>
