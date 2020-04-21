using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStagingStore : SqlStoreBase, ITransactionStagingStore
    {
        public SqlTransactionStagingStore(ReverseMyBudgetDbContext ctx) : base(ctx)
        {
        }

        public Task AddAsync(IEnumerable<TransactionStaging> transactions)
        {
            _ctx.TransactionStaging.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }
    }
}