using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
using ZipViewer.Models.Messages;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private ZipContainerEntry container;
    private readonly IFileInfoProvider infoProvider;
    private readonly IFileService fileService;

    [ObservableProperty]
    private bool isArchiveSelected;
    public ObservableCollection<ZipEntryWrapper> ContainerItems
    {
        get;
        private set;
    }
    public MainViewModel(IFileInfoProvider fileInfoProvider, IFileService fileService)
    {
        infoProvider = fileInfoProvider;
        this.fileService = fileService;
        isArchiveSelected = false;
    }

    /// <summary>
    /// Opens archive entry when executed
    /// </summary>
    /// <param name="entry"> Entry to open </param>
    [RelayCommand]
    private async Task OpenEntryAsync(ZipEntryWrapper entry)
    {
        if (entry is ZipContainerEntry openedContainer)
        {
            Messenger.Send(new NavigationRequiredMessage(openedContainer));
        } else
        {
            await fileService.StartAsync(entry);
        }
    }

    private void InitializeDirectory(ZipContainerEntry currentContainer)
    {
        // Initializing items and current container 
        container = currentContainer;
        ContainerItems = new ObservableCollection<ZipEntryWrapper>(container.InnerEntries);

        // Getting thumbnails and file types for each entry
        foreach (var entry in ContainerItems)
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
