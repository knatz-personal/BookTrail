namespace Shared.Extensions
{
    public static class ListExtensions
    {
        public enum EMoveDirection
        {
            Up,
            Down
        }

        public static List<T> MoveToTop<T>(this List<T> list, T element)
        {
            _ = list.Remove(element);
            list.Insert(0, element);

            return list;
        }
    }
}