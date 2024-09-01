using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Media.Imaging;
using Vanara.PInvoke;
using ZipViewer.Helpers.Extensions;
using ZipViewer.Models.Zip;
using Icon = System.Drawing.Icon;

namespace ZipViewer.Helpers;

public static class Win32Helper
{
    public static void ProvideWindowHandle(object target)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(target, handle);
    }

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
            var bitmap = new BitmapImage();
            bitmap.SetSource(icon);
            entry.Thumbnail = bitmap;
        }
    }
}