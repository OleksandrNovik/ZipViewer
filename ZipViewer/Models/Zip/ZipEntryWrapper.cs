using System.ComponentModel;
using System.IO.Compression;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using ZipViewer.Helpers.Extensions;
using Icon = System.Drawing.Icon;
using IOPath = System.IO.Path;

namespace ZipViewer.Models.Zip;

public partial class ZipEntryWrapper : ObservableObject, IEditableObject
{
    private ZipArchiveEntry entry;

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
        private set;
    }

    public ByteSize CompressedSize
    {
        get;
        private set;
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

    [ObservableProperty]
    private string fileType;

    public ZipEntryWrapper(ZipArchiveEntry entry)
    {
        this.entry = entry;
        Size = new ByteSize(entry.Length);
        CompressedSize = new ByteSize(entry.CompressedLength);
        ExternalAttributes = (FileAttributes)entry.ExternalAttributes;
        name = entry.Name;
        Thumbnail = new BitmapImage();
    }

    public void UpdateThumbnail(Icon thumbnailSource)
    {
        Thumbnail.SetSource(thumbnailSource);
        OnPropertyChanged(nameof(Thumbnail));
    }

    public virtual void Delete()
    {
        entry.Delete();
    }

    public async virtual Task ExtractAsync(string directory)
    {
        using (var fs = new FileStream(IOPath.Combine(directory, Name), FileMode.Create))
        {
            using (var entryStream = entry.Open())
            {
                await entryStream.CopyToAsync(fs);
            }
        }
    }

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

    public async virtual Task CopyToAsync(ZipEntryWrapper other)
    {
        other.Size = new ByteSize(entry.Length);
        other.CompressedSize = new ByteSize(entry.CompressedLength);

        using (var otherFs = other.entry.Open())
        {
            using (var thisFs = entry.Open())
            {
                await thisFs.CopyToAsync(otherFs);
            }
        }
    }
}