using ReverseMyBudget.Domain;
using System.Collections.Generic;
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

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }
    }
}