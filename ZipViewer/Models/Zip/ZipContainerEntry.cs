using System.IO.Compression;
using ZipViewer.Helpers;
using ZipViewer.Models.Contracts;
using IOPath = System.IO.Path;

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

    /// <summary>
    /// Creates directory and extracts each inner entry of this container
    /// </summary>
    /// <param name="directory"> Destination directory </param>
    public async override Task ExtractAsync(string directory)
    {
        var directoryPath = IOPath.Combine(directory, Name);
        Directory.CreateDirectory(directoryPath);

        foreach (var entry in InnerEntries)
        {
            await entry.ExtractAsync(directoryPath);
        }
    }

    /// <inheritdoc />
    public bool Contains(string name)
    {
        return InnerEntries.AsParallel().Any(entry => entry.Name == name);
    }
}