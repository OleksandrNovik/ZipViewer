using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ZipViewer.Contracts;
using ZipViewer.Helpers;
using ZipViewer.ViewModels.Contracts;
using ZipViewer.Views;

namespace ZipViewer.Services;

public sealed class NavigationService : INavigationService
{
    private Frame? frame;
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

    private void RegisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (frame != null)
        {
            frame.Navigated -= OnNavigated;
        }
    }

    public void Navigate(object parameter)
    {
        ArgumentNullException.ThrowIfNull(Frame);
        var previousViewModel = Frame.GetPageViewModel();

        bool navigated = Frame.NavigateToType(typeof(MainPage), parameter, new FrameNavigationOptions
        {
            IsNavigationStackEnabled = false
        });

        if (navigated)
        {
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
            if (navFrame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }
        }
    }
}