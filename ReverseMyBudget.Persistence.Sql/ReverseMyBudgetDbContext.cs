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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Transaction>(b =>
                {
                    // In case the transactions have already been imported, we don't want to
                    // import them again and corrupt the data
                    b.HasIndex(i => new { i.UserId, i.DateLocal, i.Amount, i.Description })
                        .IsUnique();
                });
        }
    }
}