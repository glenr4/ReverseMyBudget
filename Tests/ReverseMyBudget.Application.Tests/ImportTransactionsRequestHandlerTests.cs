using AutoFixture;
using FluentAssertions;
using Moq;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence;
using ReverseMyBudget.Persistence.Sql;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReverseMyBudget.Application.Tests
{
    public class ImportTransactionsRequestHandlerTests
    {
        private readonly Mock<ITransactionStagingStore> _transactionStagingStore;
        private readonly Mock<ITransactionConverter> _transactionConverter;
        private readonly Mock<IUserProvider> _userProvider;
        private readonly Mock<ILogger> _logger;
        private readonly Guid _userId;
        private readonly Fixture _fixture;

        public ImportTransactionsRequestHandlerTests()
        {
            _transactionStagingStore = new Mock<ITransactionStagingStore>();
            _transactionConverter = new Mock<ITransactionConverter>();
            _userProvider = new Mock<IUserProvider>();
            _logger = new Mock<ILogger>();

            _userId = Guid.NewGuid();
            _userProvider.Setup(u => u.UserId).Returns(_userId);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task WhenRequest_TransactionCoverterCalledForEachLineAndAddToStore()
        {
            //Arrange
            var lineCount = 6;
            var lines = this.CreateRandomStringArray(lineCount);
            var file = this.CreateStreamFromStrings(lines);
            var accountId = Guid.NewGuid();

            var request = new ImportTransactionsRequest
            {
                File = file,
                AccountId = accountId,
                FileName = _fixture.Create<string>()
            };

            var transaction = _fixture.Create<TransactionStaging>();
            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()
                ))
                .Returns(transaction);

            // Act
            ImportTransactionsRequest.Handler handler = this.CreateHandler();
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            var expectedTransactions = new List<TransactionStaging>();
            foreach (var line in lines)
            {
                _transactionConverter.Verify(t => t.Convert(_userId, accountId, line), Times.Once);

                expectedTransactions.Add(transaction);
            }

            _transactionStagingStore.Verify(t =>
                t.AddAsync(It.Is<List<TransactionStaging>>(x =>
                    x.ShouldBeEquivalentTrue(expectedTransactions))));
        }

        [Fact]
        public async Task WhenRequest_IfTransactionIsNullDontAddToStore()
        {
            // Arrange
            var lineCount = 2;
            var lines = this.CreateRandomStringArray(lineCount);
            var file = this.CreateStreamFromStrings(lines);
            var accountId = Guid.NewGuid();

            var request = new ImportTransactionsRequest
            {
                File = file,
                AccountId = accountId,
                FileName = _fixture.Create<string>()
            };

            var expectedTransaction = new TransactionStaging();
            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.Is<string>(l => l == lines[0])
                ))
                .Returns(expectedTransaction);

            var expectedTransactions = new List<TransactionStaging>
            {
                expectedTransaction,
            };

            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.Is<string>(l => l == lines[1])
                ))
                .Returns(default(TransactionStaging)); // null

            _transactionStagingStore.Setup(t => t.AddAsync(It.IsAny<IEnumerable<TransactionStaging>>()))
                .ReturnsAsync(_fixture.Create<int>());

            // Act
            var handler = this.CreateHandler();
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            _transactionStagingStore.Verify(t => t.AddAsync(expectedTransactions), Times.Once);
        }

        private ImportTransactionsRequest.Handler CreateHandler()
        {
            return new ImportTransactionsRequest.Handler(
                _transactionStagingStore.Object,
                _transactionConverter.Object,
                _userProvider.Object,
                _logger.Object);
        }

        private Stream CreateStreamFromStrings(params string[] values)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            foreach (string value in values)
            {
                writer.Write(value + Environment.NewLine);
            }
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        private string[] CreateRandomStringArray(int length)
        {
            var list = new List<string>();

            for (int i = 0; i < length; i++)
            {
                list.Add(_fixture.Create<string>());
            }

            return list.ToArray();
        }
    }
}