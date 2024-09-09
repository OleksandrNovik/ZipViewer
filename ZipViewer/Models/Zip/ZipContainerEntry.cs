using System.IO.Compression;
using ZipViewer.Helpers;
using ZipViewer.Models.Contracts;

namespace ZipViewer.Models.Zip;

public sealed class ZipContainerEntry : ZipEntryWrapper, IEntriesContainer
{
    public List<ZipEntryWrapper> InnerEntries
    {
        get;
    }
    public ZipContainerEntry(ZipArchiveEntry entry) : base(entry)
    {
        Name = PathHelper.GetArchiveDirectoryName(entry.FullName);
        InnerEntries = [];
    }

    /// <inheritdoc />
    public override void Delete()
    {
        // Delete each item inside directory
        foreach (var item in InnerEntries)
        {
            item.Delete();
        }

        // Delete directory itself 
        base.Delete();
    }

    /// <inheritdoc />
    public bool Contains(string name)
    {
        return InnerEntries.AsParallel().Any(entry => entry.Name == name);
    }
}