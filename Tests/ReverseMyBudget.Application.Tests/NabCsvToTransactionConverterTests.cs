using FluentAssertions;
using Moq;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Application.Tests
{
    public class NabCsvToTransactionConverterTests
    {
        private ILogger _logger;

        public NabCsvToTransactionConverterTests()
        {
            _logger = Mock.Of<ILogger>();
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithDateLocalUsingFirstItem(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().NotBeNull();
            result.DateLocal.Should().Be(Convert.ToDateTime(data[0]));
        }

        [Theory]
        [InlineData(",2,3,4,5,6,7")]
        [InlineData("1,2,3,4,5,6,7")]
        [InlineData("sdfsadf,2,3,4,5,6,7")]
        public void GivenDateLocalCannotBeParsed_WhenConvert_ThenReturnNull(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithAmountUsingSecondItem(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().NotBeNull();
            result.Amount.Should().Be(Convert.ToDecimal(data[1]));
        }

        [Theory]
        [InlineData("1,,3,4,5,6,7")]
        [InlineData("1,sdfsadf,3,4,5,6,7")]
        public void GivenAmountCannotBeParsed_WhenConvert_ThenReturnNull(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithTypeUsingFifthItem(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().NotBeNull();
            result.Type.Should().Be(data[4]);
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithDescriptionUsingSixthItem(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().NotBeNull();
            result.Description.Should().Be(data[5]);
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithBalanceUsingSeventhItem(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().NotBeNull();
            result.Balance.Should().Be(Convert.ToDecimal(data[6]));
        }

        [Theory]
        [InlineData("1,2,3,4,5,6,")]
        [InlineData("1,2,3,4,5,6,sdfsadf")]
        public void GivenBalanceCannotBeParsed_WhenConvert_ThenReturnNull(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(ValidLines))]
        public void GivenLineHas7Items_WhenConvert_ThenTransactionReturnedWithIdUserIdAndAccountId(string line)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(userId, accountId, line);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.UserId.Should().Be(userId);
            result.AccountId.Should().Be(accountId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1,2")]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,4")]
        [InlineData("1,2,3,4,5")]
        [InlineData("1,2,3,4,5,6")]
        [InlineData("1,2,3,4,5,6,7,8")]
        [InlineData("1,2,3,4,5,6,7,8,9")]
        public void GivenLineHasMoreOrLessThan7Items_WhenConvert_ThenNullReturned(string line)
        {
            // Arrange

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GivenStartsWithDate_WhenConvert_ThenNullReturned()
        {
            // Arrange
            string line = "Date,2,3,4,5,6,7";

            // Act
            var converter = new NabCsvToTransactionConverter(_logger);
            var result = converter.Convert(Guid.NewGuid(), Guid.NewGuid(), line);

            // Assert
            string[] data = line.Split(",", StringSplitOptions.None);

            result.Should().BeNull();
        }

        public static IEnumerable<object[]> ValidLines =>
        new List<object[]>
        {
            new object[] {"01 Apr 20,100.00,,,TRANSFER CREDIT,ONLINE  B7346795123 Weekly savings      G and G,1000.00" },
            new object[] {"31 Mar 20,0.31,,,INTEREST PAID,,1234.01"},
            new object[] {"25 Mar 20,100.00,,,TRANSFER CREDIT,ONLINE  K0350414026 Weekly savings      G and G,111.70"},
            new object[] {"18 Mar 20,456.00,'000000000000',,TRANSFER CREDIT,ONLINE N0854575810 Weekly savings G and G,456.70"},
            new object[] {"16 Mar 20,-1000.00,'000000000000',,TRANSFER DEBIT,INTERNET TRANSFER OL00027173,5678.70"},
        };

        // Also write tests for incorrect data types
    }
}