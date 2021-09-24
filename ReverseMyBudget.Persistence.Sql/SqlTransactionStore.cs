using ReverseMyBudget.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStore : ITransactionStore
    {
        private readonly ReverseMyBudgetDbContext _ctx;

        public SqlTransactionStore(ReverseMyBudgetDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<PagedList<Transaction>> GetAsync(TransactionQueryParameters parameters)
        {
            return PagedList<Transaction>.ToPagedListAsync(
                _ctx.QueryAll<Transaction>().CreatePredicate(parameters).OrderByDescending(o => o.DateLocal),
                parameters.PageNumber,
                parameters.PageSize);
        }
    }
}