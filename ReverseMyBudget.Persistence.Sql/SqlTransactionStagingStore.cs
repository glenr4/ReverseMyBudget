using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStagingStore : SqlStoreBase, ITransactionStagingStore
    {
        private readonly IUserProvider _userProvider;

        public SqlTransactionStagingStore(ReverseMyBudgetDbContext ctx, IUserProvider userProvider) : base(ctx)
        {
            _userProvider = userProvider;
        }

        public async Task AddAsync(IEnumerable<TransactionStaging> transactions)
        {
            _ctx.TransactionStaging.AddRange(transactions);

            await _ctx.SaveChangesAsync();

            int? quantity = 0;
            var userIdParam = new SqlParameter("UserId", _userProvider.UserId);
            var quantityParam = new SqlParameter();
            quantityParam.ParameterName = "RowCount";
            quantityParam.SqlDbType = SqlDbType.Int;
            quantityParam.Direction = ParameterDirection.Output;

            try
            {
                await _ctx.Database.ExecuteSqlRawAsync("EXEC " + StoredProcedureAddDistinctToTransactions.Name + " @UserId, @RowCount output", new[] { userIdParam, quantityParam });
                quantity = Convert.ToInt32(quantityParam.Value);
            }
            catch (Exception ex)
            {
            }
        }
    }
}