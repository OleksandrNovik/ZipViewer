using Windows.Storage;

namespace ZipViewer.Contracts
{
    public interface IFilePickingService
    {
        public Task<StorageFile?> OpenSingleAsync(params string[] extensions);

    }
}
