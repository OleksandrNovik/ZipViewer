using Microsoft.UI.Dispatching;
using Windows.UI.ViewManagement;
using ZipViewer.Helpers;

namespace ZipViewer;

public sealed partial class MainWindow : WindowEx
{
    private readonly DispatcherQueue dispatcherQueue;

    private readonly UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "Zip Viewer";

        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += OnSettingsColorValuesChanged;
    }

    /// <summary>
    /// This handles updating the caption button colors correctly when Windows system theme is changed
    /// while the app is open
    /// </summary>
    private void OnSettingsColorValuesChanged(UISettings sender, object args)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}
