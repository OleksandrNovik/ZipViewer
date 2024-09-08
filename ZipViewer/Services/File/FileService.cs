using System.Diagnostics;
using ZipViewer.Contracts.File;
using ZipViewer.Models.Zip;

namespace ZipViewer.Services.File
{
    /// <summary>
    /// Service that works with archived files
    /// </summary>
    public sealed class FileService : IFileService
    {
        /// <summary>
        /// Extracts file to a provided directory
        /// </summary>
        /// <param name="wrapper"> Entry to extract to file </param>
        /// <param name="path"> Path to directory that file should be created in </param>
        /// <returns> <see cref="FileInfo"/> that represents extracted item </returns>
        /// <exception cref="FileNotFoundException"> If file wa not created throws exception </exception>
        public async Task<FileInfo> ExtractAsync(ZipEntryWrapper wrapper, string path)
        {
            await wrapper.ExtractAsync(path);
            var filePath = Path.Combine(path, wrapper.Name);

            if (!Path.Exists(filePath))
            {
                throw new FileNotFoundException("Cannot create file in temp location", filePath);
            }

            return new FileInfo(filePath);
        }

        /// <inheritdoc />
        public async Task StartAsync(ZipEntryWrapper wrapper)
        {
            // Create file in temp location 
            var tempPath = Path.GetTempPath();
            var file = await ExtractAsync(wrapper, tempPath);

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
    }
}
