using System.Linq;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlStoreBase
    {
        protected readonly ReverseMyBudgetDbContext _ctx;

        public SqlStoreBase(ReverseMyBudgetDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// All of the entities in the DbSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryAll<T>() where T : class
        {
            return _ctx.Set<T>();
        }
    }
}