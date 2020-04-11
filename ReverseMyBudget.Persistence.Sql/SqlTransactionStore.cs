using LinqKit;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
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

        public async Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters)
        {
            //var test = CreatePredicate(QueryAll<Transaction>(), parameters);

            var query = QueryAll<Transaction>();

            var test2 = await PagedList<Transaction>.ToPagedListAsync(
                //QueryAll<Transaction>().Where("Type == @0", "PURCHASE AUTHORISATION"),
                //QueryAll<Transaction>().Where("t => t.Type.Contains(@0)", "AUTHORISATION").Where(a => a.AccountId == new Guid("db64ea81-51fe-40d8-aa08-00851184b396")),
                CreatePredicate(query, parameters),
                parameters.PageNumber,
                parameters.PageSize);

            return test2;
        }

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }

        public IQueryable<T1> CreatePredicate<T1, T2>(IQueryable<T1> queryable, T2 parameters)
        {
            foreach (var property in typeof(T2).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite))
            {
                // string contains
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(parameters);
                    if (value != default)   // only query if property has a value
                    {
                        var query = $"q => q.{property.Name}.Contains(@0)";

                        queryable = queryable.Where(query, value);
                    }
                }

                // >= StartDate  or <= EndDate
                if (property.PropertyType == typeof(DateRange))
                {
                    var value = (DateRange)property.GetValue(parameters);
                    if (value?.StartDate != default)   // only query if property has a value
                    {
                        string query = $"q => q.{property.Name} >= @0";
                        queryable = queryable.Where(query, value.StartDate);
                    }
                    if (value?.EndDate != default)   // only query if property has a value
                    {
                        string query = $"q => q.{property.Name} <= @0";
                        queryable = queryable.Where(query, value.EndDate);
                    }
                }
            }

            return queryable;
        }
    }
}