using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace ReverseMyBudget.Persistence.Sql
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T1> CreatePredicate<T1, T2>(this IQueryable<T1> queryable, T2 parameters)
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