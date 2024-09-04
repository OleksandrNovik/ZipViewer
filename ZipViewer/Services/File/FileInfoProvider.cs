using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services.File;

/// <summary>
/// Realization of information provider that provides thumbnail and other information for zip entry
/// </summary>
public sealed class FileInfoProvider : IFileInfoProvider
{
    /// <inheritdoc />
    public void GetFileInfoForItem(ZipEntryWrapper entry)
    {
        Win32Helper.GetInfoForEntry(entry);
    }
}