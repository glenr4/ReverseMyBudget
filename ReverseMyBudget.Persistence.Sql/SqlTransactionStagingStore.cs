using ReverseMyBudget.Domain;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStagingStore : ITransactionStagingStore
    {
        private readonly ReverseMyBudgetDbContext _ctx;
        private readonly IAddDistinctToTransactions _addDistinctToTransactions;
        private readonly ILogger _logger;

        public SqlTransactionStagingStore(
            ReverseMyBudgetDbContext ctx,
            IAddDistinctToTransactions addDistinctToTransactions,
            ILogger logger)
        {
            _ctx = ctx;
            _addDistinctToTransactions = addDistinctToTransactions;
            _logger = logger;
        }

        public async Task<int> AddAsync(IEnumerable<TransactionStaging> transactions)
        {
            _logger.Information($"{nameof(SqlTransactionStagingStore)} adding transactions");

            _ctx.TransactionStaging.AddRange(transactions);

            await _ctx.SaveChangesAsync();

            return await _addDistinctToTransactions.Execute();
        }
    }
}