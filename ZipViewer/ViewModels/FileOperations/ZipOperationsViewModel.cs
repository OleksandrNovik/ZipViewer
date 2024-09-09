using System.Collections.ObjectModel;
using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts.File;
using ZipViewer.Models.Messages;
using ZipViewer.Models.Zip;

namespace ZipViewer.ViewModels.FileOperations;

/// <summary>
/// Main view model for zip operations, includes logic to operate zip entries and archives
/// </summary>
public sealed partial class ZipOperationsViewModel : ObservableRecipient
{
    private readonly IFilePickingService picker;
    private readonly IFileInfoProvider infoProvider;
    private readonly IFileService fileService;

    [ObservableProperty]
    private bool canCrateItem;

    private ZipContainerEntry container;
    public ZipContainerEntry Container
    {
        get => container;
        set
        {
            if (container != value)
            {
                container = value;
                CanCrateItem = true;
                ContainerItems = new ObservableCollection<ZipEntryWrapper>(container.InnerEntries);
            }
        }
    }

    public ObservableCollection<ZipEntryWrapper> ContainerItems
    {
        get;
        private set;
    }

    /// <summary>
    /// Selected archive items that are currently operated
    /// </summary>
    public ObservableCollection<ZipEntryWrapper> SelectedItems
    {
        get;
    }

    public ZipOperationsViewModel()
    {
        canCrateItem = false;
        fileService = App.GetService<IFileService>();
        picker = App.GetService<IFilePickingService>();
        infoProvider = App.GetService<IFileInfoProvider>();

        SelectedItems = [];
        SelectedItems.CollectionChanged += OnCollectionChanged;

        App.MainWindow.Closed += OnApplicationClosing;
    }

    private void OnApplicationClosing(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
    {
        fileService.DisposeArchive();
    }

    private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OpenSelectedCommand.NotifyCanExecuteChanged();
        DeleteSelectedEntriesCommand.NotifyCanExecuteChanged();
        CopySelectedCommand.NotifyCanExecuteChanged();
    }

    private bool HasSelectedItems() => SelectedItems.Count > 0;

    /// <summary>
    /// Opens .zip file using picker
    /// </summary>
    [RelayCommand]
    private async Task OpenArchive()
    {
        // Dispose previous archive if needed
        fileService.DisposeArchive();

        // Pick up .zip file to open
        var file = await picker.OpenSingleFileAsync(".zip");

        // User selected file
        if (file is not null)
        {
            // Open .zip file
            fileService.WorkingArchive = ZipFile.Open(file.Path, ZipArchiveMode.Update);

            Messenger.Send(new ArchiveOpenedMessage(fileService.WorkingArchive));
        }
    }

    [RelayCommand]
    private void ZipDirectory()
    {
    }

    private async Task AddEntryAsync()
    {
    }

    /// <summary>
    /// Opens archive entry when executed
    /// </summary>
    /// <param name="entry"> Entry to open </param>
    [RelayCommand]
    public async Task OpenEntryAsync(ZipEntryWrapper entry)
    {
        if (entry is ZipContainerEntry openedContainer)
        {
            Messenger.Send(new NavigationRequiredMessage(openedContainer));
        } else
        {
            await fileService.StartAsync(entry);
        }
    }

    /// <summary>
    /// Opens first selected item. Can be executed only if user selected any item
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private async Task OpenSelectedAsync() => await OpenEntryAsync(SelectedItems[0]);

    [RelayCommand]
    private void CreateFile() => CreateEntry(false);

    [RelayCommand]
    private void CreateFolder() => CreateEntry(true);

    /// <summary>
    /// Adds new entry to archive
    /// </summary>
    /// <param name="isDirectory"> Is added entry a directory entry </param>
    private void CreateEntry(bool isDirectory)
    {
        var wrapper = fileService.CreateEntry(Container, "New Entry", isDirectory);

        ContainerItems.Insert(0, wrapper);

        infoProvider.GetFileInfoForItem(wrapper);
    }

    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private void BeginRenamingSelected() => BeginRenaming(SelectedItems[0]);

    private void BeginRenaming(ZipEntryWrapper entry) => entry.BeginEdit();

    [RelayCommand]
    private async Task EndRenaming(ZipEntryWrapper entry)
    {
        //TODO: check if name is valid

        entry.EndEdit();
    }

    /// <summary>
    /// Deletes selected items
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private void DeleteSelectedEntries()
    {
        // Items to delete
        var count = SelectedItems.Count;

        // Still have items to delete
        while (count > 0)
        {
            // Delete entry in archive
            SelectedItems[0].Delete();
            // Remove it from UI
            DeleteEntry(SelectedItems[0]);
            count--;
        }
    }

    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private async Task CopySelectedAsync()
    {
        foreach (var entry in SelectedItems)
        {
            var copy = await fileService.CopyEntryAsync(Container, entry.Name, entry);
            InsertEntry(copy);
        }
    }

    private void DeleteEntry(ZipEntryWrapper entry)
    {
        ContainerItems.Remove(entry);
        Container.InnerEntries.Remove(entry);
        SelectedItems.Remove(entry);
    }

    private void InsertEntry(ZipEntryWrapper entry)
    {
        // Add to UI items 
        ContainerItems.Insert(0, entry);

        // Update thumbnail and type 
        infoProvider.GetFileInfoForItem(entry);
    }
}
