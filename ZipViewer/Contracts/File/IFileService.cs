using System.IO.Compression;
using ZipViewer.Models.Contracts;
using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts.File;

public interface IFileService
{
    /// <summary>
    /// Currently opened archive
    /// </summary>
    public ZipArchive WorkingArchive
    {
        get;
        set;
    }

    /// <summary>
    /// Extracts zip entry to a provided location 
    /// </summary>
    /// <param name="wrapper"> Entry to extract </param>
    /// <param name="path"> Path to directory that item needs to be extracted into </param>
    /// <returns> <see cref="FileInfo"/> that represents extracted item </returns>
    public Task<FileInfo> ExtractAsync(ZipEntryWrapper wrapper, string path);

    /// <summary>
    /// Starts zip entry in default application 
    /// </summary>
    /// <param name="wrapper"> Entry that needs to be started </param>
    public Task StartAsync(ZipEntryWrapper wrapper);
    public string GenerateUniqueName(IEntriesContainer container, string namePattern);

    /// <summary>
    /// Creates new archive entry inside provided container
    /// </summary>
    /// <param name="creationContainer"> Directory to create entry into </param>
    /// <param name="itemName"> Name of item that is created </param>
    /// <param name="isDirectory"> Is new item a directory or a file </param>
    /// <returns> Wrapper of created item </returns>
    public ZipEntryWrapper CreateEntry(ZipContainerEntry creationContainer, string itemName, bool isDirectory);

    public Task<ZipEntryWrapper> CopyEntryAsync(ZipContainerEntry destinationFolder, string copyName, ZipEntryWrapper source);

    public Task<ZipEntryWrapper> CutEntryAsync(ZipContainerEntry destinationFolder, string copyName, ZipEntryWrapper source);

    /// <summary>
    /// Disposes archive if it's necessary
    /// </summary>
    public void DisposeArchive();

}