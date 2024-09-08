using ZipViewer.Contracts.Navigation;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services.Navigation;

/// <summary>
/// Service to handle saving of navigation history
/// </summary>
public sealed class NavigationHistoryService : INavigationHistoryService<ZipContainerEntry>
{
    public NavigationHistoryService()
    {
        ForwardStack = [];
        BackStack = [];
    }

    /// <inheritdoc />
    public Stack<ZipContainerEntry> ForwardStack
    {
        get;
    }

    /// <inheritdoc />
    public Stack<ZipContainerEntry> BackStack
    {
        get;
    }

    /// <inheritdoc />
    public ZipContainerEntry Current
    {
        get;
        private set;
    }

    /// <inheritdoc />
    public bool CanGoBack => BackStack.Count > 0;

    /// <inheritdoc />
    public bool CanGoForward => ForwardStack.Count > 0;

    /// <inheritdoc />
    public void GoForward()
    {
        BackStack.Push(Current);
        Current = ForwardStack.Pop();
    }

    /// <inheritdoc />
    public void GoForward(ZipContainerEntry navigationItem)
    {
        Current = navigationItem;
    }

    /// <inheritdoc />
    public void GoBack()
    {
        ForwardStack.Push(Current);
        Current = BackStack.Pop();
    }

    public void GoBack(ZipContainerEntry navigationItem)
    {
        BackStack.Push(Current);
        Current = navigationItem;
    }

    /// <inheritdoc />
    public void Open(ZipContainerEntry navigationItem)
    {
        ForwardStack.Clear();

        // Can occur when opening archive from file (no navigation was executed previously)
        if (Current is not null)
        {
            BackStack.Push(Current);
        }

        Current = navigationItem;
    }

    /// <inheritdoc />
    public void Clear()
    {
        ForwardStack.Clear();
        BackStack.Clear();
    }
}
