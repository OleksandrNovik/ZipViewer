using System.IO.Compression;
using ZipViewer.Contracts;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services;

public sealed class ZipHierarchyBuilder : IZipHierarchyBuilder
{

    public ZipContainerEntry BuildHierarchy(ZipArchive archive)
    {
        using (var entriesEnumerator = archive.Entries.GetEnumerator())
        {
            entriesEnumerator.MoveNext();
            // Creating root of a currently checked directory
            var root = new ZipContainerEntry(entriesEnumerator.Current);

            BuildForCurrentRoot(root, entriesEnumerator);

            return root;
        }
    }

    private void BuildForCurrentRoot(ZipContainerEntry root, IEnumerator<ZipArchiveEntry> enumerator)
    {
        // Entries that belong to directory
        var entries = new List<ZipEntryWrapper>();

        // Get key for this directory that items need to contain in their fullname to be inside this directory
        var directoryKey = PathHelper.DirectoryKeyFromArchiveKey(root.Path);


        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;

            // If archive entry's path does not contain directory's key - it belongs to other directory
            if (!current.FullName.Contains(directoryKey))
            {
                break;
            }

            // If entry has name - it is a file inside archive
            if (!string.IsNullOrEmpty(current.Name))
            {
                // We can just add it to current archive directory
                entries.Add(new ZipEntryWrapper(current));

            } else
            {
                // If it is entry that should contain other entries we should check it too
                var subDirectory = new ZipContainerEntry(current);
                BuildForCurrentRoot(subDirectory, enumerator);

                entries.Add(subDirectory);
            }
        }

        root.InnerEntries = entries;
    }
}