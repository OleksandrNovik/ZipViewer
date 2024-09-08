using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ZipViewer.Contracts.Navigation;
using ZipViewer.Helpers.Extensions;
using ZipViewer.Models.Zip;
using ZipViewer.ViewModels.Contracts;
using ZipViewer.Views;

namespace ZipViewer.Services.Navigation;

/// <summary>
/// Service that performs navigation between pages
/// </summary>
public sealed class NavigationService : INavigationService
{
    private Frame? frame;

    /// <inheritdoc />
    public Frame? Frame
    {
        get => frame;
        set
        {
            UnregisterFrameEvents();
            frame = value;
            RegisterFrameEvents();
        }
    }

    /// <summary>
    /// Register to a frame navigation event
    /// </summary>
    private void RegisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated += OnNavigated;
        }
    }

    /// <summary>
    /// Unregister frame navigation event
    /// </summary>
    private void UnregisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated -= OnNavigated;
        }
    }
    /// <inheritdoc />
    public void Navigate(ZipContainerEntry entry)
    {
        ArgumentNullException.ThrowIfNull(Frame);
        var previousViewModel = Frame.GetPageViewModel();

        var navigated = Frame.NavigateToType(typeof(MainPage), entry, new FrameNavigationOptions
        {
            IsNavigationStackEnabled = false
        });

        if (navigated)
        {
            // Notify view model that its page is being navigated from
            if (previousViewModel is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }
    }
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame navFrame)
        {
            // Notify view model that its page was navigated
            if (navFrame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }
        }
    }
}