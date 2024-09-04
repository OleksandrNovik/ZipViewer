using Microsoft.UI.Xaml.Data;

namespace ZipViewer.UI.Converters;

public sealed class DateOffsetToFormatConverter : IValueConverter
{
    public string Format
    {
        get;
        set;
    }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var str = string.Empty;

        if (value is DateTimeOffset date)
        {
            str = date.ToString(Format);
        }

        return str;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}