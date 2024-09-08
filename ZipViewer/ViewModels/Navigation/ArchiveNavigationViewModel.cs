using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts.Navigation;
using ZipViewer.Models.Messages;
using ZipViewer.Models.Zip;

namespace ZipViewer.ViewModels.Navigation;

/// <summary>
/// View model that performs navigation operations through archive folders 
/// </summary>
public sealed partial class ArchiveNavigationViewModel : ObservableRecipient
{
    /// <summary>
    /// Navigation service to navigate items when some ui options are chosen
    /// </summary>
    private readonly INavigationService navigation;

    /// <summary>
    /// History service to save each step in applications navigation history,
    /// so user can access previously navigated items
    /// </summary>
    private readonly INavigationHistoryService<ZipContainerEntry> navigationHistory;

    /// <summary>
    /// Stores parts of the route for breadcrumb bar to easily navigate to up-folders 
    /// </summary>
    public ObservableCollection<ZipContainerEntry> Route
    {
        get;
    }

    public ArchiveNavigationViewModel(INavigationService navigation, INavigationHistoryService<ZipContainerEntry> navigationHistory)
    {
        this.navigation = navigation;
        this.navigationHistory = navigationHistory;

        Route = [];

        Messenger.Register<ArchiveNavigationViewModel, ArchiveRootOpened>(this, (_, message) =>
        {
            Open(message.Root, true);
        });

        Messenger.Register<ArchiveNavigationViewModel, NavigationRequiredMessage>(this, (_, message) =>
        {
            Open(message.Container, false);
        });
    }

    /// <summary>
    /// Moves forward for one container that saved in history (if it is possible)
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanGoForward))]
    private void MoveForward()
    {
        // Mark operation as "Go forward" to a history service
        navigationHistory.GoForward();

        // Navigating current item
        NavigateCurrent();

        Route.Add(navigationHistory.Current);
    }
    private bool CanGoForward() => navigationHistory.CanGoForward;

    /// <summary>
    /// Moves back for one container that saved in history (if it is possible)
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void MoveBack()
    {
        // Mark operation as "Go back" to a history service
        navigationHistory.GoBack();

        // Navigating current item
        NavigateCurrent();

        Route.RemoveAt(Route.Count - 1);
    }
    private bool CanGoBack() => navigationHistory.CanGoBack;

    /// <summary>
    /// Navigates parent container item for a current folder (if can)
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateUp))]
    private void NavigateParent()
    {
        // If Parent property is null something went wrong
        ArgumentNullException.ThrowIfNull(navigationHistory.Current.Parent);

        // Go back with parameter to make sure user can navigate back if he needs 
        navigationHistory.GoBack(navigationHistory.Current.Parent);

        NavigateCurrent();

        // Remove last part of route, because we navigated one folder back
        Route.RemoveAt(Route.Count - 1);
    }
    private bool CanNavigateUp() => navigationHistory.Current?.Parent is not null;

    /// <summary>
    /// Uses navigating bar to navigate to a specific item
    /// </summary>
    /// <param name="lastElementIndex"> Index of last item in navigation bar </param>
    [RelayCommand]
    private void UseNavigationBar(int lastElementIndex)
    {
        // Remove each item that has bigger index that navigated item
        for (var i = Route.Count - 1; i > lastElementIndex; i--)
        {
            Route.RemoveAt(i);
        }

        // Navigate to selected item
        navigationHistory.GoForward(Route.Last());

        NavigateCurrent();
    }

    /// <summary>
    /// Navigates current item
    /// </summary>
    private void NavigateCurrent()
    {
        navigation.Navigate(navigationHistory.Current);
        NotifyCanExecute();
    }

    private void Open(ZipContainerEntry folder, bool clearHistory)
    {
        if (clearHistory)
        {
            navigationHistory.Clear();
        }

        navigationHistory.Open(folder);
        NavigateCurrent();
        Route.Add(folder);
    }

    /// <summary>
    /// Notifies commands to check if they can be executed when conditions are changed
    /// </summary>
    private void NotifyCanExecute()
    {
        MoveBackCommand.NotifyCanExecuteChanged();
        MoveForwardCommand.NotifyCanExecuteChanged();
        NavigateParentCommand.NotifyCanExecuteChanged();
    }

}
