using ReverseMyBudget.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStore : SqlStoreBase, ITransactionStore
    {
        public SqlTransactionStore(ReverseMyBudgetDbContext ctx) : base(ctx)
        {
        }

        public Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters)
        {
            return PagedList<Transaction>.ToPagedListAsync(
                QueryAll<Transaction>().CreatePredicate(parameters),
                parameters.PageNumber,
                parameters.PageSize);
        }

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }
    }
}