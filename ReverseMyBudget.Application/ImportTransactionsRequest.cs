using MediatR;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence;
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
            private readonly ITransactionStore _transactionStore;
            private readonly ITransactionConverter _transactionConverter;
            private readonly IUserProvider _userProvider;
            private readonly ILogger _logger;

            public Handler(
                ITransactionStore transactionStore,
                ITransactionConverter transactionConverter,
                IUserProvider userProvider,
                ILogger logger)
            {
                _transactionStore = transactionStore;
                _transactionConverter = transactionConverter;
                _userProvider = userProvider;
                _logger = logger;
            }

            public async Task<int> Handle(ImportTransactionsRequest request, CancellationToken cancellationToken)
            {
                _logger.Information("{@request}", request);

                var transactions = new List<Transaction>();
                string line = "";
                int lineCount = 0;

                using (StreamReader sr = new StreamReader(request.File))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var transaction = _transactionConverter.Convert(_userProvider.UserId, request.AccountId, line);

                        if (transaction != null)
                        {
                            transactions.Add(transaction);
                            lineCount++;
                        }
                    }
                }

                await _transactionStore.AddAsync(transactions);

                return lineCount;
            }
        }
    }
}