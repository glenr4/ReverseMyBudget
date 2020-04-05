using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Persistence.Sql.Tests
{
    public class SqlTransactionStoreTests
    {
        private Fixture _fixture;

        public SqlTransactionStoreTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task WhenAdd_ThenTransactionsAddedToStore()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                _fixture.Create<Transaction>(),
                _fixture.Create<Transaction>(),
                _fixture.Create<Transaction>(),
                _fixture.Create<Transaction>(),
                _fixture.Create<Transaction>(),
            };

            // Act
            using (var ctx = this.CreateDbContext())
            {
                var store = new SqlTransactionStore(ctx);

                await store.AddAsync(transactions);
            }

            // Assert
            using (var ctx = this.CreateDbContext())
            {
                var transactionsInDb = await ctx.Transaction.ToListAsync();

                transactionsInDb.Should().BeEquivalentTo(transactions);
            }
        }

        private ReverseMyBudgetDbContext CreateDbContext()
        {
            return new ReverseMyBudgetDbContext(
                        new DbContextOptionsBuilder<ReverseMyBudgetDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDB")
                            .Options);
        }
    }
}