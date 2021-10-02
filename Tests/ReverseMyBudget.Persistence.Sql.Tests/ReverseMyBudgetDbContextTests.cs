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
    public class ReverseMyBudgetDbContextTests
    {
        private Fixture _fixture;
        private Mock<IUserProvider> _user1Provider;
        private Mock<IUserProvider> _user2Provider;
        private string _databaseName;

        public ReverseMyBudgetDbContextTests()
        {
            _fixture = new Fixture();

            _user1Provider = new Mock<IUserProvider>();
            _user2Provider = new Mock<IUserProvider>();

            _databaseName = _fixture.Create<string>();
        }

        [Fact]
        public async Task GivenMultipleUsersWithData_WhenGetTransactions_ThenOnlyDataForThatUserReturned()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            _user1Provider.Setup(u => u.UserId).Returns(userId1);
            _user2Provider.Setup(u => u.UserId).Returns(userId2);

            var user1Transaction = _fixture.Create<Transaction>();
            user1Transaction.UserId = userId1;

            var user2Transaction = _fixture.Create<Transaction>();
            user2Transaction.UserId = userId2;

            // Act
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                ctx.Add(user1Transaction);

                ctx.SaveChanges();
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                ctx.Add(user2Transaction);

                ctx.SaveChanges();
            }

            // Assert
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                var transactions = await ctx.Transaction.ToListAsync();

                transactions.Should().BeEquivalentTo(new List<Transaction> { user1Transaction },
                    options => options.Excluding(x => x.Account));
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                var transactions = await ctx.Transaction.ToListAsync();

                transactions.Should().BeEquivalentTo(new List<Transaction> { user2Transaction },
                    options => options.Excluding(x => x.Account));
            }
        }

        [Fact]
        public async Task GivenMultipleUsersWithData_WhenGetTransactionStagings_ThenOnlyDataForThatUserReturned()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            _user1Provider.Setup(u => u.UserId).Returns(userId1);
            _user2Provider.Setup(u => u.UserId).Returns(userId2);

            var user1TransactionStaging = _fixture.Create<TransactionStaging>();
            user1TransactionStaging.UserId = userId1;

            var user2TransactionStaging = _fixture.Create<TransactionStaging>();
            user2TransactionStaging.UserId = userId2;

            // Act
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                ctx.Add(user1TransactionStaging);

                ctx.SaveChanges();
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                ctx.Add(user2TransactionStaging);

                ctx.SaveChanges();
            }

            // Assert
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                var TransactionStagings = await ctx.TransactionStaging.ToListAsync();

                TransactionStagings.Should().BeEquivalentTo(new List<TransactionStaging> { user1TransactionStaging });
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                var TransactionStagings = await ctx.TransactionStaging.ToListAsync();

                TransactionStagings.Should().BeEquivalentTo(new List<TransactionStaging> { user2TransactionStaging });
            }
        }

        [Fact]
        public async Task GivenMultipleUsersWithData_WhenGetAccounts_ThenOnlyDataForThatUserReturned()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            _user1Provider.Setup(u => u.UserId).Returns(userId1);
            _user2Provider.Setup(u => u.UserId).Returns(userId2);

            var user1Account = _fixture.Create<Account>();
            user1Account.UserId = userId1;

            var user2Account = _fixture.Create<Account>();
            user2Account.UserId = userId2;

            // Act
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                ctx.Add(user1Account);

                ctx.SaveChanges();
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                ctx.Add(user2Account);

                ctx.SaveChanges();
            }

            // Assert
            using (var ctx = this.CreateDbContext(_user1Provider.Object))
            {
                var Accounts = await ctx.Account.ToListAsync();

                Accounts.Should().BeEquivalentTo(new List<Account> { user1Account });
            }

            using (var ctx = this.CreateDbContext(_user2Provider.Object))
            {
                var Accounts = await ctx.Account.ToListAsync();

                Accounts.Should().BeEquivalentTo(new List<Account> { user2Account });
            }
        }

        private ReverseMyBudgetDbContext CreateDbContext(IUserProvider userProvider)
        {
            return new ReverseMyBudgetDbContext(
                        new DbContextOptionsBuilder<ReverseMyBudgetDbContext>()
                            .UseInMemoryDatabase(databaseName: _databaseName)
                            .Options,
                        userProvider);
        }
    }
}