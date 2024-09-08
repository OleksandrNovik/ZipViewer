using CommunityToolkit.WinUI.UI.Controls;
using ZipViewer.Models.Zip;
using ZipViewer.UI.Behaviors.BaseBehaviors;

namespace ZipViewer.UI.Behaviors.DataGridBehaviors;

/// <summary>
/// Behavior that runs command only with <see cref="ZipEntryWrapper"/> command parameter 
/// </summary>
public sealed class DataGridItemDoubleClickBehavior : BaseDoubleTappedTypeSafeBehavior<DataGrid, ZipEntryWrapper>;
