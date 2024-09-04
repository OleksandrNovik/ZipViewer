using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZipViewer.UI.UserControls;

public sealed partial class ToolBarButton : UserControl
{
    public ICommand Command
    {
        get;
        set;
    }
    public int IconSize
    {
        get;
        set;
    }
    public string IconSource
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    }

    public ToolBarButton()
    {
        this.InitializeComponent();
    }
}