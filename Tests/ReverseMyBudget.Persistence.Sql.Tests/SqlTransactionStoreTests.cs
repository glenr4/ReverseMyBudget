using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Persistence.Sql.Tests
{
    public class SqlTransactionStoreTests
    {
        private Fixture _fixture;
        private Mock<IUserProvider> _userProvider;
        private string _databaseName;

        public SqlTransactionStoreTests()
        {
            _fixture = new Fixture();

            _userProvider = new Mock<IUserProvider>();

            _databaseName = _fixture.Create<string>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task WhenGetWithPageQueryParameters_ThenPagedTransactionsReturned(int pageNumber)
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userProvider.Setup(u => u.UserId).Returns(userId);

            var transactions = new List<Transaction>
            {
                CreateTransaction(userId),
                CreateTransaction(userId),
                CreateTransaction(userId),
                CreateTransaction(userId),
                CreateTransaction(userId),
                CreateTransaction(userId),
            };

            // Act
            using (var ctx = this.CreateDbContext())
            {
                ctx.AddRange(transactions);

                await ctx.SaveChangesAsync();
            }

            // Assert
            using (var ctx = this.CreateDbContext())
            {
                var store = new SqlTransactionStore(ctx);

                var queryParameters = new TransactionQueryParameters
                {
                    PageNumber = pageNumber,
                    PageSize = 2
                };

                var result = await store.GetAsync(queryParameters);

                result.Should().HaveCount(queryParameters.PageSize);
                result.CurrentPage.Should().Be(queryParameters.PageNumber);
                result.TotalPages.Should().Be(transactions.Count / queryParameters.PageSize);
                result.PageSize.Should().Be(queryParameters.PageSize);
                result.TotalCount.Should().Be(transactions.Count);

                if (pageNumber == 1)
                {
                    result.HasPrevious.Should().BeFalse();
                    result.HasNext.Should().BeTrue();
                }
                if (pageNumber == 2)
                {
                    result.HasPrevious.Should().BeTrue();
                    result.HasNext.Should().BeTrue();
                }
                if (pageNumber == 3)
                {
                    result.HasPrevious.Should().BeTrue();
                    result.HasNext.Should().BeFalse();
                }
            }
        }

        private ReverseMyBudgetDbContext CreateDbContext()
        {
            return new ReverseMyBudgetDbContext(
                        new DbContextOptionsBuilder<ReverseMyBudgetDbContext>()
                            .UseInMemoryDatabase(databaseName: _databaseName)
                            .Options,
                        _userProvider.Object);
        }

        private SqlTransactionStore CreateStore(ReverseMyBudgetDbContext ctx)
        {
            return new SqlTransactionStore(ctx);
        }

        private Transaction CreateTransaction(Guid userId)
        {
            var transaction = _fixture.Create<Transaction>();

            transaction.UserId = userId;

            return transaction;
        }
    }
}