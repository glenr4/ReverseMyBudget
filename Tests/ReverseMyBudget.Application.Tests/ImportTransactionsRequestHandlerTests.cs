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
    public class ImportTransactionsRequestHandlerTests
    {
        private readonly Mock<ITransactionStore> _transactionStore;
        private readonly Mock<ITransactionConverter> _transactionConverter;
        private readonly Mock<IUserProvider> _userProvider;
        private readonly Mock<ILogger> _logger;
        private readonly Guid _userId;
        private readonly IRandomizerString _randomText;

        public ImportTransactionsRequestHandlerTests()
        {
            _transactionStore = new Mock<ITransactionStore>();
            _transactionConverter = new Mock<ITransactionConverter>();
            _userProvider = new Mock<IUserProvider>();
            _logger = new Mock<ILogger>();

            _userId = Guid.NewGuid();
            _userProvider.Setup(u => u.UserId).Returns(_userId);

            _randomText = RandomizerFactory.GetRandomizer(new FieldOptionsText());
        }

        [Fact]
        public async Task WhenRequest_TransactionCoverterCalledForEachLine()
        {
            // Arrange
            var lineCount = 6;
            var lines = this.CreateRandomStringArray(lineCount);
            var file = this.CreateStreamFromStrings(lines);
            var accountId = Guid.NewGuid();

            var request = new ImportTransactionsRequest
            {
                File = file,
                AccountId = accountId,
                FileName = _randomText.Generate()
            };

            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()
                ))
                .Returns(new Transaction());

            // Act
            var handler = this.CreateHandler();
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            foreach (var line in lines)
            {
                _transactionConverter.Verify(t => t.Convert(_userId, accountId, line), Times.Once);
            }
        }

        [Fact]
        public async Task WhenRequest_IfTransactionIsNullDontAddToStoreAndReturnCountOfAddedToStore()
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
                FileName = _randomText.Generate()
            };

            var expectedTransaction = new Transaction();
            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.Is<string>(l => l == lines[0])
                ))
                .Returns(expectedTransaction);

            _transactionConverter.Setup(t => t.Convert(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.Is<string>(l => l == lines[1])
                ))
                .Returns(default(Transaction)); // null

            _transactionStore.Setup(t => t.AddAsync(It.IsAny<IEnumerable<Transaction>>()))
                .Returns(Task.CompletedTask);

            // Act
            var handler = this.CreateHandler();
            var result = await handler.Handle(request, new CancellationToken());

            // Assert
            var expectedTransactions = new List<Transaction> { expectedTransaction };
            _transactionStore.Verify(t => t.AddAsync(expectedTransactions), Times.Once);

            result.Should().Be(expectedTransactions.Count);
        }

        private ImportTransactionsRequest.Handler CreateHandler()
        {
            return new ImportTransactionsRequest.Handler(
                _transactionStore.Object,
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
                list.Add(_randomText.Generate());
            }

            return list.ToArray();
        }
    }
}