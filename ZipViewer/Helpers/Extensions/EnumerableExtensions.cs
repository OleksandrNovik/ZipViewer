namespace ZipViewer.Helpers.Extensions;

public static class EnumerableExtensions
{
    public static void AddRange<T>(this IList<T> destination, IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            destination.Add(item);
        }
    }
}