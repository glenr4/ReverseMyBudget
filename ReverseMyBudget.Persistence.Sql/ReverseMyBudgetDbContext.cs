using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;

namespace ReverseMyBudget.Persistence.Sql
{
    /// <summary>
    /// Application Data.
    /// Note: UserId filtering is applied by this class
    /// </summary>
    public class ReverseMyBudgetDbContext : DbContext
    {
        private readonly IUserProvider _userProvider;

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionStaging> TransactionStaging { get; set; }

        public ReverseMyBudgetDbContext(
            DbContextOptions<ReverseMyBudgetDbContext> options,
            IUserProvider userProvider)
            : base(options)
        {
            _userProvider = userProvider;
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
                    // However, this will reject all transacations, even if only one is duplicated
                    //b.HasIndex(i => new { i.UserId, i.DateLocal, i.Amount, i.Description })
                    //    .IsUnique();

                    b.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId);

                    // Always filter by UserId
                    b.HasQueryFilter(e => _userProvider.UserId == e.UserId);
                });

            modelBuilder
                .Entity<TransactionStaging>(b =>
                {
                    // Always filter by UserId
                    b.HasQueryFilter(e => _userProvider.UserId == e.UserId);
                });

            modelBuilder
                .Entity<Account>(b =>
                {
                    // Always filter by UserId
                    b.HasQueryFilter(e => _userProvider.UserId == e.UserId);
                });
        }
    }
}