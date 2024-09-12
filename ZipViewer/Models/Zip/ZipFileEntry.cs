using System.IO.Compression;

namespace ZipViewer.Models.Zip;

/// <summary>
/// Entry that is designed to contain logic of copying and opening file zip entry
/// </summary>
public sealed class ZipFileEntry : ZipEntryWrapper
{
    public ZipFileEntry(ZipArchiveEntry entry) : base(entry)
    {
    }

    /// <summary>
    /// Opens up entries file stream
    /// </summary>
    public Stream Open() => entry.Open();

    /// <summary>
    /// Copies entry's bytes to a provided stream
    /// </summary>
    /// <param name="stream"> Stream destination that bytes are getting copied into </param>
    public async Task CopyToStreamAsync(Stream stream)
    {
        using (var fs = entry.Open())
        {
            await fs.CopyToAsync(stream);
        }
    }

    /// <summary>
    /// Extracts entry to a file and copies all bytes from entry to a created file
    /// </summary>
    /// <param name="directory"> Directory in which file should be created </param>
    public async override Task ExtractAsync(string directory)
    {
        using (var fs = new FileStream(System.IO.Path.Combine(directory, Name), FileMode.Create))
        {
            await CopyToStreamAsync(fs);
        }
    }

    /// <summary>
    /// Copies bytes of this entry to other entry provided 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public async Task CopyToAsync(ZipFileEntry other)
    {
        other.Size = new ByteSize(Size);
        other.CompressedSize = new ByteSize(CompressedSize);

        using (var otherFs = other.Open())
        {
            await CopyToStreamAsync(otherFs);
        }
    }
}
