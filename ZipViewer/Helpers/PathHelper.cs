namespace ZipViewer.Helpers;

public static class PathHelper
{
    public static string GetArchiveDirectoryName(string path)
    {
        var trimmed = path.TrimEnd('/');

        var lastSlashIndex = trimmed.LastIndexOf('/');

        return trimmed.Substring(lastSlashIndex + 1);
    }

    public static string DirectoryKeyFromArchiveKey(string path)
    {
        var lastSlashIndex = path.LastIndexOf('/');

        if (lastSlashIndex > 0)
        {
            return path.Substring(0, lastSlashIndex);
        }

        throw new ArgumentException($"Provided path \"{path}\" does not contain '/'", nameof(path));
    }
}