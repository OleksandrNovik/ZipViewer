using ZipViewer.Contracts;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services;

public sealed class FileInfoProvider : IFileInfoProvider
{
    public void GetFileInfoForItem(ZipEntryWrapper entry)
    {
        Win32Helper.GetInfoForEntry(entry);
    }
}