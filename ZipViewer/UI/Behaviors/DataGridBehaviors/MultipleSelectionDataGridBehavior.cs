using System.Collections;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using ZipViewer.Helpers.Extensions;

namespace ZipViewer.UI.Behaviors.DataGridBehaviors;
public sealed class MultipleSelectionDataGridBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(nameof(SelectedItems), typeof(IList),
            typeof(MultipleSelectionDataGridBehavior), new PropertyMetadata(null));

    public IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        // When behavior is detached that means, that page is navigated from,
        // so clearing selected items can be done here
        // This is not the best solution, but it works
        SelectedItems.Clear();
        AssociatedObject.SelectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedItems.RemoveRange(e.RemovedItems);
        SelectedItems.AddRange(e.AddedItems);
    }
}
