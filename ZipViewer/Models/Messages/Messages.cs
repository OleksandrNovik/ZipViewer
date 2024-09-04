using System.IO.Compression;

namespace ZipViewer.Models.Messages;

public record ArchiveOpenedMessage(ZipArchive Archive);
