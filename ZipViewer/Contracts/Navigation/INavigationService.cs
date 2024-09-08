using Microsoft.UI.Xaml.Controls;
using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts.Navigation;

/// <summary>
/// Contract for service that performs page to page navigation 
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigation frame for navigation between pages
    /// </summary>
    public Frame? Frame
    {
        get;
        set;
    }

    /// <summary>
    /// Navigates to next folder page for archive item
    /// </summary>
    /// <param name="entry"> New entry to navigate </param>
    public void Navigate(ZipContainerEntry entry);
}