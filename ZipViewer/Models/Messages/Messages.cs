using System.IO.Compression;
using ZipViewer.Models.Zip;

namespace ZipViewer.Models.Messages;

/// <summary>
/// Message that notifies listeners that new archive has been opened
/// </summary>
/// <param name="Archive"> Archive that is opened </param>
public record ArchiveOpenedMessage(ZipArchive Archive);

/// <summary>
/// Message that notifies navigation view model that new .zip file is opened, so history of navigation should be updated
/// </summary>
/// <param name="Root"> Root directory of .zip file </param>
public record ArchiveRootOpened(ZipContainerEntry Root);

/// <summary>
/// Message that requires navigation operation to open provided container
/// </summary>
/// <param name="Container"> Container to open and save to navigation history </param>
public record NavigationRequiredMessage(ZipContainerEntry Container);