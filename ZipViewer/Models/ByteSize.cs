﻿namespace ZipViewer.Models;

public sealed class ByteSize
{
    public static readonly string[] Sizes =
    [
        "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "RB", "QB"
    ];

    private string friendlyValue;
    public long InBytes
    {
        get;
    }

    public ByteSize(long inBytes)
    {
        InBytes = inBytes;
        friendlyValue = Convert(inBytes);
    }

    private string Convert(double value)
    {
        var converted = value;
        var order = 0;

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