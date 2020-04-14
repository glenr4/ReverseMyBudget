using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ReverseMyBudget.Persistence.Sql.Extensions
{
    public static class DbSetExtensions
    {
        public static EntityEntry<T> AddIfNotExists<T>(
        this DbSet<T> dbSet,
        T entity,
        Expression<Func<T, bool>> predicate,
        ILogger log)
        where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();

            if (exists)
            {
                log.Debug("{Method}: entity already exists {@entity}", nameof(AddIfNotExists), entity);
            }
            else
            {
                log.Debug("{Method}: adding entity {@entity}", nameof(AddIfNotExists), entity);
            }

            return !exists ? dbSet.Add(entity) : null;
        }
    }
}