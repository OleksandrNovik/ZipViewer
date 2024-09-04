using Microsoft.UI.Xaml.Controls;
using ZipViewer.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZipViewer.UI.UserControls;
public sealed partial class ZipOperationsToolbar : UserControl
{
    public ZipOperationsViewModel ViewModel
    {
        get;
    }
    public ZipOperationsToolbar()
    {
        ViewModel = App.GetService<ZipOperationsViewModel>();
        this.InitializeComponent();
    }
}
