using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;

namespace ZipViewer.Models.Zip;

public class ZipEntryWrapper : ObservableObject
{
    private ZipArchiveEntry entry;

    public string Name
    {
        get;
        protected set;
    }

    public string Path => entry.FullName;
    public DateTimeOffset LastChange => entry.LastWriteTime;

    public string Comment
    {
        get => entry.Comment;
        set
        {
            if (entry.Comment != value)
            {
                entry.Comment = value;
                OnPropertyChanged();
            }
        }
    }

    public ByteSize Size
    {
        get;
    }

    public ByteSize CompressedSize
    {
        get;
    }

    public FileAttributes ExternalAttributes
    {
        get;
        protected set;
    }

    public BitmapImage Thumbnail
    {
        get;
        protected set;
    }

    public ZipEntryWrapper(ZipArchiveEntry entry)
    {
        this.entry = entry;
        Size = new ByteSize(entry.Length);
        CompressedSize = new ByteSize(entry.CompressedLength);
        ExternalAttributes = (FileAttributes)entry.ExternalAttributes;
        Name = entry.Name;
    }

    public virtual void Delete()
    {
        entry.Delete();
    }
}