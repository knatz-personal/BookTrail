using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Shared.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Applies pagination to a query
        /// </summary>
        public static IQueryable<T> ApplyPaging<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        ///     Applies ordering to a query
        /// </summary>
        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            string sortBy,
            bool sortDescending,
            Dictionary<string, Expression<Func<T, object>>> sortExpressions)
        {
            if (string.IsNullOrWhiteSpace(sortBy) || !sortExpressions.ContainsKey(sortBy))
            {
                // Use first available expression as default
                Expression<Func<T, object>> defaultExpression = sortExpressions.FirstOrDefault().Value;
                return defaultExpression != null
                    ? sortDescending ? query.OrderByDescending(defaultExpression) : query.OrderBy(defaultExpression)
                    : query;
            }

            Expression<Func<T, object>> expression = sortExpressions[sortBy];
            return sortDescending
                ? query.OrderByDescending(expression)
                : query.OrderBy(expression);
        }

        /// <summary>
        ///     Creates a paginated result from a query
        /// </summary>
        public static async Task<(IEnumerable<T> Items, int TotalCount, int PageCount)>
            ToPaginatedResultAsync<T>(
                this IQueryable<T> query,
                int pageNumber,
                int pageSize)
        {
            int totalCount = await query.CountAsync();
            int pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            List<T> items = await query
                .ApplyPaging(pageNumber, pageSize)
                .ToListAsync();

            return (items, totalCount, pageCount);
        }
    }
}