using System.Collections.ObjectModel;
using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts.File;
using ZipViewer.Helpers;
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
        CopySelectedCommand.NotifyCanExecuteChanged();
        CutSelectedCommand.NotifyCanExecuteChanged();
        DeleteSelectedEntriesCommand.NotifyCanExecuteChanged();
        ExtractSelectedCommand.NotifyCanExecuteChanged();
    }

    private bool HasSelectedItems() => SelectedItems.Count > 0;

    private bool HasCopiedItems() => ZipClipboard.Any;

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

    /// <summary>
    /// Adds entries from files that were picked
    /// </summary>
    [RelayCommand]
    private async Task AddFileEntriesAsync()
    {
        //Get files from picker with any type
        var files = await picker.OpenMultipleFilesAsync("*");

        // Archive them and convert them to wrappers 
        var wrappers = await fileService.CrateFileEntriesAsync(files, Container);

        // Add to UI list
        foreach (var wrapper in wrappers)
        {
            InsertEntry(wrapper);
        }
    }

    [RelayCommand]
    private async Task AddDirectoryEntryAsync()
    {
        // The same for directory get directory items
        var directory = await picker.OpenSingleFolderAsync();

        var wrapper = await fileService.CreateContainerEntryAsync(directory, Container);

        InsertEntry(wrapper);
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
        } else if (entry is ZipFileEntry file)
        {
            await fileService.StartAsync(file);
        } else
        {
            throw new ArgumentException("Could not open entry", nameof(entry));
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
        var newEntry = await fileService.CutEntryAsync(Container, entry.Name, entry);
        InsertEntry(newEntry);
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
            RemoveEntry(SelectedItems[0]);
            count--;
        }
    }

    /// <summary>
    /// Copies selected items to application's clipboard
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private void CopySelected()
    {
        ZipClipboard.SaveItems(SelectedItems.ToArray(), CopyOperation.Copy);
        PasteCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Cuts selected items to application's clipboard
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private void CutSelected()
    {
        ZipClipboard.SaveItems(SelectedItems.ToArray(), CopyOperation.Cut);

        PasteCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(HasSelectedItems))]
    private async Task ExtractSelectedAsync()
    {
        var destination = await picker.OpenSingleFolderAsync();

        await Parallel.ForEachAsync(SelectedItems, async (entry, token) =>
        {
            await entry.ExtractAsync(destination.Path);
        });
    }

    /// <summary>
    /// Pastes each entry in clipboard to current container
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasCopiedItems))]
    private async Task PasteAsync()
    {
        // Get items and operation from clipboard
        var (items, operation) = ZipClipboard.GetItems();

        foreach (var entry in items)
        {
            ZipEntryWrapper copy;

            if (operation == CopyOperation.Copy)
            {
                copy = await fileService.CopyEntryAsync(Container, entry.Name, entry);

            } else
            {
                copy = await fileService.CutEntryAsync(Container, entry.Name, entry);
                // Since items are cut notify that there are no items in clipboard 
                PasteCommand.NotifyCanExecuteChanged();

                // Remove item if exists from container (to prevent pasting item that was cut in same directory)
                ContainerItems.Remove(entry);
            }

            InsertEntry(copy);
        }
    }

    /// <summary>
    /// Deletes (not physically) entry from UI collections and container it is held in
    /// </summary>
    /// <param name="entry"> Entry to delete (not physically)</param>
    private void RemoveEntry(ZipEntryWrapper entry)
    {
        ContainerItems.Remove(entry);
        Container.InnerEntries.Remove(entry);
        SelectedItems.Remove(entry);
    }

    /// <summary>
    /// Inserts entry into UI container items and updates its UI data
    /// </summary>
    /// <param name="entry"> Entry to insert </param>
    private void InsertEntry(ZipEntryWrapper entry)
    {
        // Add to UI items 
        ContainerItems.Insert(0, entry);

        // Update thumbnail and type 
        infoProvider.GetFileInfoForItem(entry);
    }
}
