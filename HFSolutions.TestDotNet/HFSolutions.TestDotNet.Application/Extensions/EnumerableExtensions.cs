using System.Linq.Expressions;

namespace HFSolutions.TestDotNet.Application.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> values, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                values = values.AsQueryable().Where(predicate);
            }

            return values;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                query = query.Where(predicate);
            }

            return query;
        }
    }
}
