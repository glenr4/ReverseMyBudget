using LinqKit;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStore : SqlStoreBase, ITransactionStore
    {
        public SqlTransactionStore(ReverseMyBudgetDbContext ctx) : base(ctx)
        {
        }

        //public Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters)
        //{
        //    var predicate = PredicateBuilder.New<Transaction>(true);
        //    predicate = predicate.And(t => t.Description.Contains(parameters.Description));

        //    var test = new QueryParamsToPredicateBuilder();
        //    test.ReadEntity(parameters);

        //    return PagedList<Transaction>.ToPagedListAsync(
        //        QueryAll<Transaction>(),
        //        predicate,
        //        parameters.PageNumber,
        //        parameters.PageSize);
        //}

        public Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters)
        {
            return PagedList<Transaction>.ToPagedListAsync(
                //QueryAll<Transaction>().Where("Type == @0", "PURCHASE AUTHORISATION"),
                QueryAll<Transaction>().Where("t => t.Type.Contains(@0)", "AUTHORISATION"),
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