using System.Collections;

namespace ZipViewer.Helpers.Extensions;

public static class EnumerableExtensions
{
    public static void AddRange(this IList destination, IEnumerable source)
    {
        foreach (var item in source)
        {
            destination.Add(item);
        }
    }

    public static void RemoveRange(this IList destination, IEnumerable source)
    {
        foreach (var item in source)
        {
            destination.Remove(item);
        }
    }

    public static void AddRange<T>(this IList<T> destination, IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            destination.Add(item);
        }
    }
}