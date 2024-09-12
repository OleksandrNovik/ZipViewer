using Windows.Storage;
using Windows.Storage.Pickers;
using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
using ZipViewer.Helpers.Extensions;

namespace ZipViewer.Services.File;

/// <summary>
/// Service that provides file picker functionality
/// </summary>
public class FilePickingService : IFilePickingService
{
    /// <inheritdoc />
    public async Task<StorageFile?> OpenSingleFileAsync(params string[] extensions)
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.ComputerFolder
        };

        picker.FileTypeFilter.AddRange(extensions);

        Win32Helper.ProvideWindowHandle(picker);

        return await picker.PickSingleFileAsync();
    }

    public async Task<IReadOnlyCollection<StorageFile>> OpenMultipleFilesAsync(params string[] extensions)
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.ComputerFolder
        };

        picker.FileTypeFilter.AddRange(extensions);

        Win32Helper.ProvideWindowHandle(picker);

        return await picker.PickMultipleFilesAsync();
    }

    public async Task<StorageFolder> OpenSingleFolderAsync()
    {
        var picker = new FolderPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.ComputerFolder
        };
        picker.FileTypeFilter.Add("*");

        Win32Helper.ProvideWindowHandle(picker);

        return await picker.PickSingleFolderAsync();
    }
}