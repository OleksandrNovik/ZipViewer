using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ZipViewer.Contracts;
using ZipViewer.Contracts.Navigation;
using ZipViewer.Models.Messages;

namespace ZipViewer.ViewModels;

public sealed class ShellPageViewModel : ObservableRecipient
{
    private readonly IZipHierarchyBuilder hierarchyBuilder;
    public INavigationService Navigation
    {
        get;
    }

    public ShellPageViewModel(INavigationService navigation, IZipHierarchyBuilder hierarchyBuilder)
    {
        Navigation = navigation;
        this.hierarchyBuilder = hierarchyBuilder;

        Messenger.Register<ShellPageViewModel, ArchiveOpenedMessage>(this, (_, message) =>
        {
            // Build archive's inner structure
            var root = this.hierarchyBuilder.BuildHierarchy(message.Archive);

            Messenger.Send(new ArchiveRootOpened(root));
        });
    }
}