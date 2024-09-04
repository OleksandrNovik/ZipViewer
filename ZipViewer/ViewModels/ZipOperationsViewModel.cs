using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts;
using ZipViewer.Models.Messages;

namespace ZipViewer.ViewModels;
public sealed partial class ZipOperationsViewModel : ObservableRecipient
{
    private readonly IFilePickingService picker;
    private ZipArchive? archive;

    public ZipOperationsViewModel(IFilePickingService filePicker)
    {
        picker = filePicker;
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
