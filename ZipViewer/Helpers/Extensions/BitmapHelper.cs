using System.Drawing.Imaging;
using Microsoft.UI.Xaml.Media.Imaging;
using Icon = System.Drawing.Icon;

namespace ZipViewer.Helpers.Extensions
{
    public static class BitmapHelper
    {
        public static void SetSource(this BitmapImage bitmap, Icon icon)
        {
            using (var bmpImage = icon.ToBitmap())
            {
                using (var ms = new MemoryStream())
                {
                    bmpImage.Save(ms, ImageFormat.Png);

                    ms.Seek(0, SeekOrigin.Begin);

                    bitmap.SetSource(ms.AsRandomAccessStream());
                }
            }
        }
    }
}
