namespace Shared.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            if (items == null || items.Count() == 0)
            {
                return items;
            }

            return items.GroupBy(property).Select(x => x.First());
        }
    }
}