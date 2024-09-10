using System.Diagnostics;
using System.IO.Compression;
using ZipViewer.Contracts.File;
using ZipViewer.Models.Contracts;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services.File
{
    /// <summary>
    /// Service that works with archived files
    /// </summary>
    public sealed class FileService : IFileService
    {
        /// <inheritdoc />
        public ZipArchive WorkingArchive
        {
            get;
            set;
        }

        /// <summary>
        /// Extracts file to a provided directory
        /// </summary>
        /// <param name="wrapper"> Entry to extract to file </param>
        /// <param name="path"> Path to directory that file should be created in </param>
        /// <returns> <see cref="FileInfo"/> that represents extracted item </returns>
        /// <exception cref="FileNotFoundException"> If file wa not created throws exception </exception>
        public async Task<FileSystemInfo> ExtractAsync(ZipEntryWrapper wrapper, string path)
        {
            await wrapper.ExtractAsync(path);
            var extractedPath = Path.Combine(path, wrapper.Name);

            if (!Path.Exists(extractedPath))
            {
                throw new FileNotFoundException("Cannot create physical item", extractedPath);
            }

            return wrapper is ZipFileEntry ? new FileInfo(extractedPath) : new DirectoryInfo(extractedPath);
        }

        /// <inheritdoc />
        public async Task StartAsync(ZipFileEntry entryFile)
        {
            // Create file in temp location 
            var tempPath = Path.GetTempPath();
            var file = await ExtractAsync(entryFile, tempPath);

            try
            {
                var fileOpenProcess = new Process
                {
                    StartInfo = new ProcessStartInfo(file.FullName)
                    {
                        UseShellExecute = true
                    },

                    EnableRaisingEvents = true
                };

                // Start process to open file
                fileOpenProcess.Start();

                // Cleanup: Subscribe to process exit event to delete file on exit
                fileOpenProcess.Exited += FileProcessExited;
            }
            catch
            {
                //TODO: Notify user that item cannot be opened
            }
        }

        /// <summary>
        /// When process with opened in service file is exited, file that was created to be opened gets deleted
        /// </summary>
        /// <param name="sender"> Process that was exited </param>
        /// <param name="e"> Empty event args </param>
        private void FileProcessExited(object? sender, EventArgs e)
        {
            if (sender is Process process)
            {
                System.IO.File.Delete(process.StartInfo.FileName);
            }
        }

        /// <inheritdoc />
        public string GenerateUniqueName(IEntriesContainer container, string template)
        {
            var itemsCounter = 1;
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(template);
            var extension = Path.GetExtension(template);
            var newName = template;

            while (container.Contains(newName))
            {
                itemsCounter++;
                newName = $"{nameWithoutExtension} ({itemsCounter}){extension}";
            }

            return newName;
        }

        /// <inheritdoc />
        public ZipEntryWrapper CreateEntry(ZipContainerEntry creationContainer, string itemName, bool isDirectory)
        {
            // Generate unique name for a new item
            var name = GenerateUniqueName(creationContainer, itemName);

            // Adding to archive (if it is directory will add '/' to the end of the path
            var path = Path.Combine(creationContainer.Path, name) + (isDirectory ? "/" : "");
            var newEntry = WorkingArchive.CreateEntry(path);

            //Creating wrapper and inserting it to the start 
            ZipEntryWrapper wrapper = isDirectory ? new ZipContainerEntry(newEntry) : new ZipFileEntry(newEntry);

            creationContainer.InnerEntries.Add(wrapper);
            wrapper.Parent = creationContainer;

            return wrapper;
        }

        /// <inheritdoc />
        public async Task<ZipEntryWrapper> CopyEntryAsync(ZipContainerEntry destinationFolder, string copyName, ZipEntryWrapper source)
        {
            var sourceDirectory = source as ZipContainerEntry;
            var isDirectory = sourceDirectory is not null;
            var copy = CreateEntry(destinationFolder, copyName, isDirectory);

            // If source is directory
            if (isDirectory)
            {
                var copyDirectory = copy as ZipContainerEntry;

                // Copy each inner item of target directory to a copy directory
                foreach (var innerEntry in sourceDirectory!.InnerEntries.ToArray())
                {
                    await CopyEntryAsync(copyDirectory!, innerEntry.Name, innerEntry);
                }

            } else if (source is ZipFileEntry file)
            {
                var copyFile = copy as ZipFileEntry;

                // If it is file entry copy all the bytes
                await file.CopyToAsync(copyFile!);
            } else
            {
                throw new ArgumentException("Item is not zip wrapper", nameof(source));
            }

            return copy;
        }

        public async Task<ZipEntryWrapper> CutEntryAsync(ZipContainerEntry destinationFolder, string copyName, ZipEntryWrapper source)
        {
            // Cut is similar to copy
            var copy = await CopyEntryAsync(destinationFolder, copyName, source);

            // Only deleting source element
            source.Delete();

            return copy;
        }

        /// <inheritdoc />
        public void DisposeArchive()
        {
            if (WorkingArchive is not null)
            {
                WorkingArchive.Dispose();
            }
        }
    }
}
