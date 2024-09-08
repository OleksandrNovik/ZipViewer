using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts.File;
using ZipViewer.Models.Messages;
using ZipViewer.Models.Zip;

namespace ZipViewer.ViewModels.FileOperations;
public sealed partial class ZipOperationsViewModel : ObservableRecipient
{
    private readonly IFilePickingService picker;
    private readonly IFileService fileService;

    private ZipArchive? archive;

    public ZipOperationsViewModel()
    {
        fileService = App.GetService<IFileService>();
        picker = App.GetService<IFilePickingService>();
    }

    /// <summary>
    /// Opens .zip file using picker
    /// </summary>
    [RelayCommand]
    private async Task OpenArchive()
    {
        // Dispose previous archive if needed
        DisposeArchive();

        // Pick up .zip file to open
        var file = await picker.OpenSingleAsync(".zip");

        // User selected file
        if (file is not null)
        {
            // Open .zip file
            archive = ZipFile.OpenRead(file.Path);

            Messenger.Send(new ArchiveOpenedMessage(archive));
        }
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
    /// Disposes archive if it's necessary
    /// </summary>
    private void DisposeArchive()
    {
        if (archive is not null)
        {
            archive.Dispose();
        }
    }
}
