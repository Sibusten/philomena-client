using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Images.Downloaders
{
    /// <summary>
    /// A delegate that determines which file to save an image to
    /// </summary>
    /// <param name="image">The image being downloaded</param>
    /// <returns>The file to download the image to</returns>
    public delegate string GetFileForImageDelegate(IPhilomenaImage image);

    public class PhilomenaImageFileDownloader : PhilomenaImageDownloader
    {
        private const string _tempExtension = "tmp";

        private readonly GetFileForImageDelegate _getFileForImage;

        public PhilomenaImageFileDownloader(GetFileForImageDelegate getFileForImage)
        {
            _getFileForImage = getFileForImage;
        }

        public override async Task Download(IPhilomenaImage downloadItem, CancellationToken cancellationToken = default, IProgress<DownloadProgressInfo>? progress = null)
        {
            string file = _getFileForImage(downloadItem);

            // Create directory for image download
            string? imageDirectory = Path.GetDirectoryName(file);
            if (imageDirectory is null)
            {
                throw new DirectoryNotFoundException($"The file does not have a parent directory: {file}");
            }
            Directory.CreateDirectory(imageDirectory);

            // Create stream progress info
            IProgress<StreamProgressInfo> streamProgress = new SyncProgress<StreamProgressInfo>(streamProgress =>
            {
                progress?.Report(new DownloadProgressInfo
                {
                    Current = streamProgress.BytesRead,
                    Total = streamProgress.BytesTotal,
                    Action = downloadItem.Id.ToString(),
                });
            });

            // Get the download stream for the image
            using Stream downloadStream = await GetDownloadStream(downloadItem, cancellationToken, streamProgress);

            // Write to a temp file first
            string tempFile = file + "." + _tempExtension;
            using (FileStream tempFileStream = File.OpenWrite(tempFile))
            {
                await downloadStream.CopyToAsync(tempFileStream, cancellationToken);
            }

            // Move the temp file to the destination file
            File.Move(tempFile, file, overwrite: true);
        }
    }
}
