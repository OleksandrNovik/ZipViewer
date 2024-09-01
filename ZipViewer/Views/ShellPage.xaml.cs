using Microsoft.UI.Xaml.Controls;
using ZipViewer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZipViewer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPageViewModel ViewModel
        {
            get;
        }
        public ShellPage()
        {
            ViewModel = App.GetService<ShellPageViewModel>();
            this.InitializeComponent();

            ViewModel.Navigation.Frame = NavigationFrame;
        }
    }
}
