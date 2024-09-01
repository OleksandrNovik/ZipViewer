using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZipViewer.Contracts;

namespace ZipViewer.ViewModels;

public sealed partial class ShellPageViewModel : ObservableRecipient
{
    private readonly IFilePickingService picker;
    private readonly IZipHierarchyBuilder hierarchyBuilder;
    public INavigationService Navigation
    {
        get;
    }

    private ZipArchive? archive;

    public ShellPageViewModel(INavigationService navigation, IFilePickingService filePicker, IZipHierarchyBuilder hierarchyBuilder)
    {
        Navigation = navigation;
        picker = filePicker;
        this.hierarchyBuilder = hierarchyBuilder;
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
            InitializeArchive(ZipFile.OpenRead(file.Path));
        }
    }

    private void InitializeArchive(ZipArchive archive)
    {
        this.archive = archive;

        // Build archive's inner structure
        var root = hierarchyBuilder.BuildHierarchy(archive);

        // Navigate to a resulting root directory
        Navigation.Navigate(root);
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