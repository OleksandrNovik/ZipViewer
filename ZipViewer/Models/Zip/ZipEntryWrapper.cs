using System.ComponentModel;
using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using ZipViewer.Helpers.Extensions;
using Icon = System.Drawing.Icon;

namespace ZipViewer.Models.Zip;

public abstract partial class ZipEntryWrapper : ObservableObject, IEditableObject
{
    protected ZipArchiveEntry entry;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private bool isEditem;

    private string backUpName;

    public string Path => entry.FullName;
    public DateTimeOffset LastChange => entry.LastWriteTime;

    public ZipContainerEntry? Parent
    {
        get;
        set;
    }

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
        protected set;
    }

    public ByteSize CompressedSize
    {
        get;
        protected set;
    }

    public FileAttributes ExternalAttributes
    {
        get;
        protected set;
    }

    public BitmapImage Thumbnail
    {
        get;
        set;
    }

    public bool HasSize
    {
        get;
    }

    [ObservableProperty]
    private string fileType;

    public ZipEntryWrapper(ZipArchiveEntry entry)
    {
        this.entry = entry;
        try
        {
            Size = new ByteSize(entry.Length);
            CompressedSize = new ByteSize(entry.CompressedLength);
            HasSize = true;
        }
        // If size cannot be evaluated since entry has been opened
        catch (InvalidOperationException)
        {
            Size = ByteSize.Unknown;
            CompressedSize = ByteSize.Unknown;
            HasSize = false;
        }

        ExternalAttributes = (FileAttributes)entry.ExternalAttributes;
        name = entry.Name;
    }

    public void UpdateThumbnail(Icon thumbnailSource)
    {
        Thumbnail ??= new BitmapImage();
        Thumbnail.SetSource(thumbnailSource);
        OnPropertyChanged(nameof(Thumbnail));
    }

    public virtual void Delete()
    {
        entry.Delete();
    }

    /// <summary>
    /// Creates exact copy of entry in windows file system
    /// </summary>
    /// <param name="directory"> Directory that item should be extract into </param>
    public abstract Task ExtractAsync(string directory);

    /// <summary>
    /// Saves old name into backup and starts renaming item if it was not already renamed
    /// </summary>
    public void BeginEdit()
    {
        if (!IsEditem)
        {
            backUpName = Name;
            IsEditem = true;
        }
    }

    /// <summary>
    /// Cancels renaming by restoring old name (this method is useful when new name is invalid) 
    /// </summary>
    public void CancelEdit()
    {
        if (IsEditem)
        {
            Name = backUpName;
            IsEditem = false;
        }
    }

    /// <summary>
    /// Ends renaming item by giving it new valid name 
    /// </summary>
    public void EndEdit()
    {
        if (IsEditem)
        {
            backUpName = Name;
            IsEditem = false;
        }
    }
}