namespace ZipViewer.Helpers;

public static class Win32Helper
{
    public static void ProvideWindowHandle(object target)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(target, handle);
    }
}