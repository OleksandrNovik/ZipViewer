using Windows.Storage;

namespace ZipViewer.Contracts.File;

/// <summary>
/// Contract for a service that provides file picker
/// </summary>
public interface IFilePickingService
{
    /// <summary>
    /// Opens windows file picker for opening files
    /// </summary>
    /// <param name="extensions"> Extensions that are set to filter of picker </param>
    /// <returns> Storage item that was selected in picker menu </returns>
    public Task<StorageFile?> OpenSingleFileAsync(params string[] extensions);

}