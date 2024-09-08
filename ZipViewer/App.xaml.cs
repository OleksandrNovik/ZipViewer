using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using ZipViewer.Contracts;
using ZipViewer.Contracts.File;
using ZipViewer.Contracts.Navigation;
using ZipViewer.Helpers;
using ZipViewer.Models.Zip;
using ZipViewer.Services;
using ZipViewer.Services.File;
using ZipViewer.Services.Navigation;
using ZipViewer.ViewModels;
using ZipViewer.ViewModels.Navigation;
using ZipViewer.Views;

namespace ZipViewer;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        ThreadingHelper.InitializeForMainThread();
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            //Services 
            services.AddTransient<IZipHierarchyBuilder, ZipHierarchyBuilder>();

            //Navigation
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddTransient<INavigationHistoryService<ZipContainerEntry>, NavigationHistoryService>();

            // File services
            services.AddTransient<IFilePickingService, FilePickingService>();
            services.AddTransient<IFileInfoProvider, FileInfoProvider>();
            services.AddTransient<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<ShellPageViewModel>();
            services.AddTransient<ShellPage>();

            services.AddTransient<ZipOperationsViewModel>();
            services.AddTransient<ArchiveNavigationViewModel>();

        }).
        Build();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = MainWindow;
        m_window.Content = GetService<ShellPage>();
        m_window.Activate();
    }

    private Window m_window;
}
