using System.IO.Compression;
using ZipViewer.Models.Zip;

namespace ZipViewer.Models.Messages;

/// <summary>
/// Message that notifies listeners that new archive has been opened
/// </summary>
/// <param name="Archive"> Archive that is opened </param>
public record ArchiveOpenedMessage(ZipArchive Archive);

/// <summary>
/// Notifies listeners that new container inside archive should be opened
/// </summary>
/// <param name="Container"> Container to "open" or navigate </param>
public record ContainerEntryNavigatedMessage(ZipContainerEntry Container);