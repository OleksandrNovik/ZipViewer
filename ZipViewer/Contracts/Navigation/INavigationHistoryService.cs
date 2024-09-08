using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts.Navigation;

/// <summary>
/// Contract for navigation history generic service
/// </summary>
/// <typeparam name="TNavigationItem"> Type of item that is used in navigation history </typeparam>
public interface INavigationHistoryService<TNavigationItem>
{
    /// <summary>
    /// Archive folders that can be navigated forward
    /// </summary>
    public Stack<TNavigationItem> ForwardStack
    {
        get;
    }

    /// <summary>
    /// Archive folders that can be navigated back
    /// </summary>
    public Stack<TNavigationItem> BackStack
    {
        get;
    }

    /// <summary>
    /// Currently opened item
    /// </summary>
    public ZipContainerEntry Current
    {
        get;
    }

    /// <summary>
    /// Is there any directory that can be navigated back
    /// </summary>
    public bool CanGoBack
    {
        get;
    }

    /// <summary>
    /// Is there any directory that can be navigated forward
    /// </summary>
    public bool CanGoForward
    {
        get;
    }

    /// <summary>
    /// Method to "go forward" through history 
    /// </summary>
    public void GoForward();

    /// <summary>
    /// Method to "go forward" through history that saves provided item as current
    /// </summary>
    /// <param name="navigationItem"> Item that needs to be currently opened </param>
    public void GoForward(TNavigationItem navigationItem);

    /// <summary>
    /// Method to "go back" through history 
    /// </summary>
    public void GoBack();

    /// <summary>
    /// Method to "go back" through history that saves provided item as current
    /// </summary>
    /// <param name="navigationItem"> Item that needs to be currently opened </param>
    public void GoBack(TNavigationItem navigationItem);

    /// <summary>
    /// Opens navigation item and clears forward navigation history
    /// </summary>
    /// <param name="navigationItem"> Item that needs to be currently opened </param>
    public void Open(TNavigationItem navigationItem);

    /// <summary>
    /// Clears navigation history
    /// </summary>
    public void Clear();
}
