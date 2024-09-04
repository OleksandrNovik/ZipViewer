using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts;
using ZipViewer.Models.Messages;

namespace ZipViewer.ViewModels;

public sealed partial class ShellPageViewModel : ObservableRecipient
{
    private readonly IZipHierarchyBuilder hierarchyBuilder;
    public INavigationService Navigation
    {
        get;
    }

    private ZipArchive? archive;

    public ShellPageViewModel(INavigationService navigation, IZipHierarchyBuilder hierarchyBuilder)
    {
        Navigation = navigation;
        this.hierarchyBuilder = hierarchyBuilder;

        Messenger.Register<ShellPageViewModel, ArchiveOpenedMessage>(this, (_, message) =>
        {
            // Build archive's inner structure
            var root = this.hierarchyBuilder.BuildHierarchy(message.Archive);

            Navigation.Navigate(root);
        });
    }
}