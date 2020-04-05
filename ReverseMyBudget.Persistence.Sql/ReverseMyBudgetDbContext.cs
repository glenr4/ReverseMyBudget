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
        public DbSet<Account> Account { get; set; }

        public ReverseMyBudgetDbContext(DbContextOptions<ReverseMyBudgetDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Should we use composite PK/FK? ie Id and UserId?
            modelBuilder
                .Entity<Transaction>(b =>
                {
                    // In case the transactions have already been imported, we don't want to
                    // import them again and corrupt the data
                    b.HasIndex(i => new { i.UserId, i.DateLocal, i.Amount, i.Description })
                        .IsUnique();

                    b.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.Id);
                });
        }
    }
}