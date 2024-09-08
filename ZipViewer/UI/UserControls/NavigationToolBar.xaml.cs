using Microsoft.UI.Xaml.Controls;
using ZipViewer.ViewModels.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZipViewer.UI.UserControls;
public sealed partial class NavigationToolBar : UserControl
{
    public ArchiveNavigationViewModel ViewModel
    {
        get;
    }
    public NavigationToolBar()
    {
        ViewModel = App.GetService<ArchiveNavigationViewModel>();
        this.InitializeComponent();
    }
}
