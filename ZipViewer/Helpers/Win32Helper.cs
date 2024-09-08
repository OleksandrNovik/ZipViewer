using System.Runtime.InteropServices;
using Vanara.PInvoke;
using ZipViewer.Models.Zip;
using Icon = System.Drawing.Icon;

namespace ZipViewer.Helpers;

public static class Win32Helper
{
    /// <summary>
    /// Provides main window handle for provided target
    /// </summary>
    /// <param name="target"> Target that needs main window handle </param>
    public static void ProvideWindowHandle(object target)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(target, handle);
    }

    /// <summary>
    /// Gets file information for zip entry
    /// </summary>
    /// <param name="entry"> Entry to get file information for </param>
    public static void GetInfoForEntry(ZipEntryWrapper entry)
    {
        var shFileInfo = new Shell32.SHFILEINFO();

        Shell32.SHGetFileInfo(entry.Path, entry.ExternalAttributes, ref shFileInfo, Marshal.SizeOf(shFileInfo),
             Shell32.SHGFI.SHGFI_ICON |
             Shell32.SHGFI.SHGFI_TYPENAME |
             Shell32.SHGFI.SHGFI_LARGEICON |
             Shell32.SHGFI.SHGFI_USEFILEATTRIBUTES);

        entry.FileType = shFileInfo.szTypeName;

        using (var icon = Icon.FromHandle(shFileInfo.hIcon.DangerousGetHandle()))
        {
            entry.UpdateThumbnail(icon);
        }
    }
}