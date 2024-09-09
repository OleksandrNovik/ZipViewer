using CommunityToolkit.Mvvm.ComponentModel;
using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;
using ZipViewer.ViewModels.FileOperations;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFileInfoProvider infoProvider;
    public ZipOperationsViewModel ZipOperations
    {
        get;
    }

    [ObservableProperty]
    private bool isArchiveSelected;

    public MainViewModel(IFileInfoProvider fileInfoProvider, ZipOperationsViewModel zipOperations)
    {
        infoProvider = fileInfoProvider;
        isArchiveSelected = false;

        ZipOperations = zipOperations;
    }

    private void InitializeDirectory(ZipContainerEntry currentContainer)
    {
        // Initializing items and current container 
        ZipOperations.Container = currentContainer;

        // Getting thumbnails and file types for each entry
        foreach (var entry in ZipOperations.ContainerItems)
        {
            ThreadingHelper.TryEnqueue(() =>
            {
                infoProvider.GetFileInfoForItem(entry);
            });
        }

        // Notifying system that archive is opened now
        IsArchiveSelected = true;
    }

    public void OnNavigatedTo(object parameter)
    {
        // If navigation parameter is container of zip entries
        if (parameter is ZipContainerEntry paramContainer)
        {
            InitializeDirectory(paramContainer);
        }
    }

    public void OnNavigatedFrom()
    {
        Messenger.UnregisterAll(this);
    }
}
