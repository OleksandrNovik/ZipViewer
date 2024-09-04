using System.Drawing.Imaging;
using Microsoft.UI.Xaml.Media.Imaging;
using Icon = System.Drawing.Icon;

namespace ZipViewer.Helpers.Extensions;

/// <summary>
/// Static helper class that contains logic to work with bitmaps and imaging 
/// </summary>
public static class BitmapHelper
{
    /// <summary>
    /// Setting icon source for <see cref="BitmapImage" /> 
    /// </summary>
    /// <param name="bitmap"> Bitmap that we are setting source for </param>
    /// <param name="icon"> Icon source </param>
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