using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts.File;

public interface IFileService
{
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
}