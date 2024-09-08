using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace ZipViewer.UI.Behaviors.BaseBehaviors;

/// <summary>
/// Base class for a behaviours that have to run command on some event
/// </summary>
/// <typeparam name="T"> The object type to attach to </typeparam>
public abstract class BaseCommandBehavior<T> : Behavior<T>
    where T : DependencyObject
{
    public static DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(IRelayCommand),
            typeof(BaseCommandBehavior<T>), new PropertyMetadata(null));

    public IRelayCommand Command
    {
        get => (IRelayCommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get;
        set;
    }

}
