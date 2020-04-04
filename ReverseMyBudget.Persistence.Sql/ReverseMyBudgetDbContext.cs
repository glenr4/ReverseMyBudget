using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;

namespace ReverseMyBudget.Persistence.Sql
{
    /// <summary>
    /// Application Data
    /// </summary>
    public class ReverseMyBudgetDbContext : DbContext
    {
        public DbSet<Transaction> Transaction { get; set; }

        public ReverseMyBudgetDbContext(DbContextOptions<ReverseMyBudgetDbContext> options)
            : base(options)
        {
        }
    }
}