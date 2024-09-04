using System.IO.Compression;
using ZipViewer.Helpers;

namespace ZipViewer.Models.Zip;

public sealed class ZipContainerEntry : ZipEntryWrapper
{
    public IReadOnlyList<ZipEntryWrapper> InnerEntries
    {
        get; set;
    }
    public ZipContainerEntry(ZipArchiveEntry entry) : base(entry)
    {
        Name = PathHelper.GetArchiveDirectoryName(entry.FullName);
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }
}