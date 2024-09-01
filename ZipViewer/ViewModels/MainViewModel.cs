using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private ZipContainerEntry container;

    [ObservableProperty]
    private bool isArchiveSelected;
    public ObservableCollection<ZipEntryWrapper> ContainerItems
    {
        get;
        private set;
    }
    public MainViewModel()
    {
        isArchiveSelected = false;
    }

    public void OnNavigatedTo(object parameter)
    {
        // If navigation parameter is container of zip entries
        if (parameter is ZipContainerEntry paramContainer)
        {
            container = paramContainer;
            ContainerItems = new ObservableCollection<ZipEntryWrapper>(container.InnerEntries);
            IsArchiveSelected = true;
        }
    }

    public void OnNavigatedFrom()
    {
        Messenger.UnregisterAll(this);
    }
}
