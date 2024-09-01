using System.IO.Compression;
using ZipViewer.Models.Zip;

namespace ZipViewer.Contracts;

public interface IZipHierarchyBuilder
{
    public ZipContainerEntry BuildHierarchy(ZipArchive archive);

}