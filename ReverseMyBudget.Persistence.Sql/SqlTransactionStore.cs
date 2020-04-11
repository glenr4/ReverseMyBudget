using LinqKit;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
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

        // TODO: need to add filtering as IQueryable
        public async Task<PagedList<Transaction>> Get(Guid userId, TransactionQueryParameters parameters)
        {
            var predicate = PredicateBuilder.New<Transaction>(true);
            predicate = predicate.And(t => t.UserId == userId);
            predicate = predicate.And(t => t.Description.Contains("Coles"));

            var result = await PagedList<Transaction>.ToPagedListAsync(
                this.FindAll<Transaction>(),
                predicate,
                parameters.PageNumber,
                parameters.PageSize);

            return result;
            //return _ctx.Transaction.Where(t => t.UserId == userId).ToListAsync();
        }

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }

        public IQueryable<T> FindAll<T>() where T : class
        {
            return _ctx.Set<T>();
        }
    }
}