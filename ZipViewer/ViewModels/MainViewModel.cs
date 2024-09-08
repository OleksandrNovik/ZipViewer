using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private ZipContainerEntry container;
    private readonly IFileInfoProvider infoProvider;

    [ObservableProperty]
    private bool isArchiveSelected;
    public ObservableCollection<ZipEntryWrapper> ContainerItems
    {
        get;
        private set;
    }
    public MainViewModel(IFileInfoProvider fileInfoProvider)
    {
        infoProvider = fileInfoProvider;
        isArchiveSelected = false;
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
