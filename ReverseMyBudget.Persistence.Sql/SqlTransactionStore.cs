using ReverseMyBudget.Domain;
using System.Linq;
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
                QueryAll<Transaction>().CreatePredicate(parameters).OrderByDescending(o => o.DateLocal),
                parameters.PageNumber,
                parameters.PageSize);
        }
    }
}