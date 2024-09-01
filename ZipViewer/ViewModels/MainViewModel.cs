using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;

namespace ZipViewer.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private ZipContainerEntry container;
    public ObservableCollection<ZipEntryWrapper> ContainerItems
    {
        get;
        private set;
    }
    public MainViewModel()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ZipContainerEntry paramContainer)
        {
            container = paramContainer;
            ContainerItems = new ObservableCollection<ZipEntryWrapper>(container.InnerEntries);
        }
    }

    public void OnNavigatedFrom()
    {
        Messenger.UnregisterAll(this);
    }
}
