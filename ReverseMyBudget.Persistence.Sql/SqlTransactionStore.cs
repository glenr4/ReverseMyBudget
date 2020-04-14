using ReverseMyBudget.Domain;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStore : SqlStoreBase, ITransactionStore
    {
        private readonly ILogger _log;

        public SqlTransactionStore(ReverseMyBudgetDbContext ctx, ILogger log) : base(ctx)
        {
            _log = log;
        }

        public Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters)
        {
            return PagedList<Transaction>.ToPagedListAsync(
                QueryAll<Transaction>().CreatePredicate(parameters).OrderByDescending(o => o.DateLocal),
                parameters.PageNumber,
                parameters.PageSize);
        }

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }

        public Task AddUniqueAsync(IEnumerable<Transaction> transactions)
        {
            foreach (var t in transactions)
            {
                _ctx.Set<Transaction>()
                    .AddIfNotExists<Transaction>(t,
                    x => x.UserId == t.UserId
                         && x.DateLocal == t.DateLocal
                         && x.Amount == t.Amount
                         && x.Description == t.Description,
                    _log
                 );
            }

            return _ctx.SaveChangesAsync();
        }
    }
}