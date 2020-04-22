using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Serilog;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStagingStore : SqlStoreBase, ITransactionStagingStore
    {
        private readonly IUserProvider _userProvider;
        private readonly ILogger _logger;

        public SqlTransactionStagingStore(ReverseMyBudgetDbContext ctx, IUserProvider userProvider, ILogger logger) : base(ctx)
        {
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task AddAsync(IEnumerable<TransactionStaging> transactions)
        {
            _logger.Information($"{nameof(SqlTransactionStagingStore)} adding transactions");

            _ctx.TransactionStaging.AddRange(transactions);

            await _ctx.SaveChangesAsync();

            int? quantity = 0;
            var userIdParam = new SqlParameter(SpAddDistinctToTransactions.UserIdParam, _userProvider.UserId);

            var rowCountParam = new SqlParameter();
            rowCountParam.ParameterName = SpAddDistinctToTransactions.RowCountOutputParam;
            rowCountParam.SqlDbType = SqlDbType.Int;
            rowCountParam.Direction = ParameterDirection.Output;

            try
            {
                await _ctx.Database.ExecuteSqlRawAsync("EXEC " + SpAddDistinctToTransactions.Name + " @UserId, @RowCount output", new[] { userIdParam, rowCountParam });
                quantity = Convert.ToInt32(rowCountParam.Value);

                _logger.Information($"{nameof(SqlTransactionStagingStore)}: {quantity} unique transactions added");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{nameof(SqlTransactionStagingStore)} adding transactions failed");
            }
        }
    }
}