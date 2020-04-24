using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReverseMyBudget.Domain;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Persistence.Sql.Tests
{
    public class SqlTransactionStagingStoreTests
    {
        private Fixture _fixture;
        private Mock<IUserProvider> _userProvider;
        private Mock<IAddDistinctToTransactions> _addDistinctToTransactions;
        private Mock<ILogger> _logger;

        public SqlTransactionStagingStoreTests()
        {
            _fixture = new Fixture();

            _userProvider = new Mock<IUserProvider>();

            _addDistinctToTransactions = new Mock<IAddDistinctToTransactions>();

            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async Task WhenAdd_ThenTransactionsAddedToStore()
        {
            // Arrange
            var transactions = new List<TransactionStaging>
            {
                _fixture.Create<TransactionStaging>(),
                _fixture.Create<TransactionStaging>(),
                _fixture.Create<TransactionStaging>(),
                _fixture.Create<TransactionStaging>(),
                _fixture.Create<TransactionStaging>(),
            };

            // Act
            using (var ctx = this.CreateDbContext())
            {
                var store = CreateStore(ctx);

                await store.AddAsync(transactions);
            }

            // Assert
            using (var ctx = this.CreateDbContext())
            {
                var transactionsInDb = ctx.TransactionStaging.IgnoreQueryFilters().ToList();    // Not testing Query Filter here

                transactionsInDb.Should().BeEquivalentTo(transactions);
            }
        }

        [Fact]
        public async Task WhenAdd_ThenAddDistinctToTransactionsExecuted()
        {
            // Arrange

            // Act
            using (var ctx = this.CreateDbContext())
            {
                var store = CreateStore(ctx);

                await store.AddAsync(new List<TransactionStaging>());
            }

            // Assert
            _addDistinctToTransactions.Verify(a => a.Execute(), Times.Once);
        }

        private ReverseMyBudgetDbContext CreateDbContext()
        {
            return new ReverseMyBudgetDbContext(
                        new DbContextOptionsBuilder<ReverseMyBudgetDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDB")
                            .Options,
                        _userProvider.Object);
        }

        private SqlTransactionStagingStore CreateStore(ReverseMyBudgetDbContext ctx)
        {
            return new SqlTransactionStagingStore(
                ctx,
                _addDistinctToTransactions.Object,
                _logger.Object);
        }
    }
}