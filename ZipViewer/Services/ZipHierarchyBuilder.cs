using System.IO.Compression;
using ZipViewer.Contracts;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services;

public sealed class ZipHierarchyBuilder : IZipHierarchyBuilder
{

    /// <summary>
    /// Builds structure of folders inside provided archive 
    /// </summary>
    /// <param name="archive"> Archive that is checked for files and folders </param>
    /// <returns> Root folder of archive </returns>
    public ZipContainerEntry BuildHierarchy(ZipArchive archive)
    {
        using (var entriesEnumerator = archive.Entries.GetEnumerator())
        {
            // Set up enumerator's first element
            entriesEnumerator.MoveNext();
            // Creating root of a currently checked directory
            var root = new ZipContainerEntry(entriesEnumerator.Current)
            {
                Parent = null
            };

            BuildForCurrentRoot(root, entriesEnumerator);

            return root;
        }
    }

    /// <summary>
    /// Seeks for files and folders inside provided root folder
    /// </summary>
    /// <param name="root"> Root that is checked for inner items </param>
    /// <param name="enumerator"> Enumerator that runs through all items inside of archive </param>
    private void BuildForCurrentRoot(ZipContainerEntry root, IEnumerator<ZipArchiveEntry> enumerator)
    {
        // Entries that belong to directory
        var entries = new List<ZipEntryWrapper>();

        // Get key for this directory that items need to contain in their fullname to be inside this directory
        var directoryKey = PathHelper.DirectoryKeyFromArchiveKey(root.Path);

        // While there are items in archive 
        while (true)
        {
            var current = enumerator.Current;

            if (current is null)
            {
                break;
            }

            // If archive entry's path does not contain directory's key - it belongs to other directory
            if (!current.FullName.Contains(directoryKey))
            {
                break;
            }

            ZipEntryWrapper added = string.IsNullOrEmpty(current.Name) ?
                new ZipContainerEntry(current) : new ZipFileEntry(current);

            // Assign parent directory to added item
            added.Parent = root;
            enumerator.MoveNext();
            entries.Add(added);

            if (added is ZipContainerEntry container)
            {
                BuildForCurrentRoot(container, enumerator);
            }
        }

        // Set inner items for current folder as found items in method
        root.InnerEntries.AddRange(entries);
    }
}