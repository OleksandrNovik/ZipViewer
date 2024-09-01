using ZipViewer.Contracts;

namespace ZipViewer.ViewModels;

public sealed class ShellPageViewModel
{
    public INavigationService Navigation
    {
        get;
    }

    public ShellPageViewModel(INavigationService navigation)
    {
        Navigation = navigation;
    }
}