using ZipViewer.Models.Zip;

namespace ZipViewer.Helpers;

public enum CopyOperation
{
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

    /// <summary>
    /// Entries for operation
    /// </summary>
    private static IEnumerable<ZipEntryWrapper> items;

    /// <summary>
    /// Required operation
    /// </summary>
    private static CopyOperation operation;

    /// <summary>
    /// Saves items and operation to clipboard
    /// </summary>
    /// <param name="entries"> Items that will be copied </param>
    /// <param name="operationRequired"> Operation that is required in future </param>
    public static void SaveItems(IEnumerable<ZipEntryWrapper> entries, CopyOperation operationRequired)
    {
        items = entries;
        operation = operationRequired;
        Any = true;
    }

    /// <summary>
    /// Gets stored items and operation that is required to do
    /// </summary>
    public static (IEnumerable<ZipEntryWrapper> Items, CopyOperation Operation) GetItems()
    {
        return (Items: items, Operation: operation);
    }

}
