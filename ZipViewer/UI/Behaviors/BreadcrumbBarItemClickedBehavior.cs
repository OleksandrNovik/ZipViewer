using Microsoft.UI.Xaml.Controls;
using ZipViewer.Helpers.Extensions;
using ZipViewer.UI.Behaviors.BaseBehaviors;

namespace ZipViewer.UI.Behaviors;

/// <summary>
/// Executes command when breadcrumb ber item is clicked
/// </summary>
public sealed class BreadcrumbBarItemClickedBehavior : BaseCommandBehavior<BreadcrumbBar>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.ItemClicked += OnItemClick;
    }
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.ItemClicked -= OnItemClick;
    }

    private void OnItemClick(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        Command.ExecuteIfCan(args.Index);
    }
}
