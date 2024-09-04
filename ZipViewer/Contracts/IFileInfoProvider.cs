using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts;

/// <summary>
/// Contract for a service that provides file info for zip entries
/// </summary>
public interface IFileInfoProvider
{
    /// <summary>
    /// Extracts file info for provided zip entry
    /// </summary>
    /// <param name="entry"> Entry that we are getting information for </param>
    public void GetFileInfoForItem(ZipEntryWrapper entry);
}