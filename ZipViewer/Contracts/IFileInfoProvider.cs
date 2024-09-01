using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts;

public interface IFileInfoProvider
{
    public void GetFileInfoForItem(ZipEntryWrapper entry);
}