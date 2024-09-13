using System.IO.Compression;
using Windows.Storage;
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
    /// Extracts file zip entry to a provided location 
    /// </summary>
    /// <param name="wrapper"> Entry to extract </param>
    /// <param name="path"> Path to directory that item needs to be extracted into </param>
    /// <returns> <see cref="FileSystemInfo"/> that represents extracted item </returns>
    public Task<FileInfo> ExtractToFileAsync(ZipFileEntry wrapper, string path);

    /// <summary>
    /// Starts zip entry in default application 
    /// </summary>
    /// <param name="entryFile"> Entry file that needs to be started </param>
    public Task StartAsync(ZipFileEntry entryFile);

    /// <summary>
    /// Generates unique name for an item inside provided container
    /// </summary>
    /// <param name="container"> Container of item </param>
    /// <param name="namePattern"> Name template for item </param>
    /// <returns> Unique name for provided container </returns>
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

    public Task<IReadOnlyCollection<ZipFileEntry>> CrateFileEntriesAsync(IReadOnlyCollection<StorageFile> files,
        ZipContainerEntry inContainer);

    public Task<ZipContainerEntry> CreateContainerEntryAsync(StorageFolder folder, ZipContainerEntry inContainer);


}