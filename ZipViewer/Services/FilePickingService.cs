using Windows.Storage;
using Windows.Storage.Pickers;
using ZipViewer.Contracts;
using ZipViewer.Helpers;
using ZipViewer.Helpers.Extensions;

namespace ZipViewer.Services
{
    public class FilePickingService : IFilePickingService
    {
        public async Task<StorageFile?> OpenSingleAsync(params string[] extensions)
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


    }
}
