using System.IO.Compression;

namespace ZipViewer.Models.Zip;

public sealed class ZipContainerEntry : ZipEntryWrapper
{
    public IReadOnlyCollection<ZipEntryWrapper> InnerEntries
    {
        get; set;
    }
    public ZipContainerEntry(ZipArchiveEntry entry) : base(entry)
    {
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }
}