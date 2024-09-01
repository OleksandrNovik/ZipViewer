using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage.Pickers;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    public MainViewModel()
    {
    }

    [RelayCommand]
    private async Task OpenArchive()
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.PicturesLibrary
        };
        picker.FileTypeFilter.Add(".zip");

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        var file = await picker.PickSingleFileAsync();


        using (var archive = ZipFile.OpenRead(file.Path))
        {
            var info = new ZipEntryWrapper(archive.Entries.First());
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        throw new NotImplementedException();
    }

    public void OnNavigatedFrom()
    {
        Messenger.UnregisterAll(this);
    }
}
