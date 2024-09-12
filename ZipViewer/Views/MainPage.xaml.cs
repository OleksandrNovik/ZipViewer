using Microsoft.UI.Xaml.Controls;
using ZipViewer.ViewModels;

namespace ZipViewer.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
