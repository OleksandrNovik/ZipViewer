using Windows.Storage;

namespace ZipViewer.Helpers.Extensions
{
    public static class StorageFileExtensions
    {
        public static bool IsArchive(this StorageFile file)
        {
            // Hash set with archives
            HashSet<string> archiveExtensions = [".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz"];

            // Check if item containing in the set
            return archiveExtensions.Contains(file.FileType.ToLower());
        }
    }
}
