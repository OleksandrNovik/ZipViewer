namespace ZipViewer.Models;

public enum ByteUnits
{
    Bytes, KBytes, MBytes, GBytes, TBytes, PBytes, EBytes, ZBytes, YBytes, RBytes, QBytes
}
public sealed class ByteSize
{
    public static readonly string[] Sizes =
    [
        "Bytes", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "RB", "QB"
    ];

    private string friendlyValue;
    public long InBytes
    {
        get;
    }

    public ByteSize(long inBytes)
    {
        InBytes = inBytes;
        friendlyValue = Convert(inBytes, ByteUnits.Bytes);
    }

    public ByteSize(double value, ByteUnits units)
    {
        var power = (int)units;
        InBytes = (long)(value * Math.Pow(1024, power));
        friendlyValue = Convert(value, units);
    }

    private string Convert(double value, ByteUnits units)
    {
        var converted = value;
        var order = (int)units;

        while (converted > 1024)
        {
            order++;
            converted /= 1024;
        }

        return $"{Math.Floor(converted)} {Sizes[order]}";
    }

    public override string ToString()
    {
        return friendlyValue;
    }
}