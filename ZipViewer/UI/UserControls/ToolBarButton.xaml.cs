using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZipViewer.UI.UserControls;

public sealed partial class ToolBarButton : UserControl
{
    /// <summary>
    /// Dependency property to check command value property changed
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand),
            typeof(ToolBarButton), new PropertyMetadata(null, OnCommandPropertyChanged));

    /// <summary>
    /// When command property changes we initialize button's executing state 
    /// </summary>
    private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ToolBarButton button && e.NewValue is ICommand command)
        {
            command.CanExecuteChanged += button.OnCanExecuteChanged;
            var canExecute = command.CanExecute(null);
            button.Icon.Opacity = canExecute ? 1 : 0.5;
        }
    }

    /// <summary>
    /// Changing opacity of icon if button is enabled or disabled (button's command can or cannot execute)
    /// </summary>
    private void OnCanExecuteChanged(object? sender, EventArgs e)
    {
        var canExecute = Command.CanExecute(null);

        Icon.Opacity = canExecute ? 1 : 0.5;
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
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