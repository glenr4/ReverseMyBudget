using MediatR;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence.Sql;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReverseMyBudget.Application
{
    public class ImportTransactionsRequest : IRequest<int>
    {
        public Stream File { get; set; }
        public Guid AccountId { get; set; }
        public string FileName { get; set; }

        public class Handler : IRequestHandler<ImportTransactionsRequest, int>
        {
            private readonly ITransactionStagingStore _transactionStagingStore;
            private readonly ITransactionConverter _transactionConverter;
            private readonly IUserProvider _userProvider;
            private readonly ILogger _logger;

            public Handler(
                ITransactionStagingStore transactionStagingStore,
                ITransactionConverter transactionConverter,
                IUserProvider userProvider,
                ILogger logger)
            {
                _transactionStagingStore = transactionStagingStore;
                _transactionConverter = transactionConverter;
                _userProvider = userProvider;
                _logger = logger;
            }

            public async Task<int> Handle(ImportTransactionsRequest request, CancellationToken cancellationToken)
            {
                _logger.Information("{@request}", request);

                var transactions = new List<TransactionStaging>();
                string line = "";
                int lineCount = 1;

                using (StreamReader sr = new StreamReader(request.File))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var transaction = _transactionConverter.Convert(_userProvider.UserId, request.AccountId, line);

                        if (transaction != null)
                        {
                            transaction.ImportOrder = lineCount;
                            lineCount++;

                            transactions.Add(transaction);
                        }
                    }
                }

                return await _transactionStagingStore.AddAsync(transactions);
            }
        }
    }
}