using ZipViewer.Helpers.Extensions;
using ZipViewer.Models.Zip;

namespace ZipViewer.Helpers;

public enum CopyOperation
{
    None,
    Copy,
    Cut
}
/// <summary>
/// Simple class to store entries that need to be copy or cut
/// </summary>
public static class ZipClipboard
{
    public static bool Any
    {
        get;
        private set;
    }

    private static ZipContainerEntry currentContainer;

    /// <summary>
    /// Entries for operation
    /// </summary>
    private static ICollection<ZipEntryWrapper> items;

    /// <summary>
    /// Required operation
    /// </summary>
    private static CopyOperation operation;

    /// <summary>
    /// Saves items and operation to clipboard
    /// </summary>
    /// <param name="entries"> Items that will be copied </param>
    /// <param name="operationRequired"> Operation that is required in future </param>
    public static void SaveItems(ICollection<ZipEntryWrapper> entries, CopyOperation operationRequired)
    {
        items = entries;
        operation = operationRequired;
        Any = true;

        if (entries.FirstOrDefault()?.Parent is { } container)
        {
            currentContainer = container;

        } else
        {
            throw new ArgumentException("Cannot copy root directory");
        }
    }


    /// <summary>
    /// Gets stored items and operation that is required to do
    /// </summary>
    public static (ICollection<ZipEntryWrapper> Items, CopyOperation Operation) GetItems()
    {
        if (operation is CopyOperation.Cut)
        {
            // Remove items if they are cut
            currentContainer.InnerEntries.RemoveRange(items);
        }

        return (Items: items, Operation: operation);
    }

    public static void Clear()
    {
        currentContainer = default;
        items = default;
        operation = CopyOperation.None;
        Any = false;
    }

}
