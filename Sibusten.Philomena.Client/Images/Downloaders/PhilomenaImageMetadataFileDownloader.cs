using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Images.Downloaders
{
    public class PhilomenaImageMetadataFileDownloader : PhilomenaImageDownloader
    {
        private const string _tempExtension = "tmp";

        private readonly GetFileForImageDelegate _getFileForImage;

        public PhilomenaImageMetadataFileDownloader(GetFileForImageDelegate getFileForImage)
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

            // Metadata is already downloaded, so just report 0 or 1 for progress
            void reportProgress(bool isFinished)
            {
                progress?.Report(new DownloadProgressInfo
                {
                    Current = isFinished ? 1 : 0,
                    Total = 1,
                    Action = $"{downloadItem.Id} Metadata",
                });
            }

            reportProgress(isFinished: false);

            // Write to a temp file first
            string tempFile = file + "." + _tempExtension;
            await File.WriteAllTextAsync(tempFile, downloadItem.RawMetadata, Encoding.UTF8, cancellationToken);

            // Move the temp file to the destination file
            File.Move(tempFile, file, overwrite: true);

            reportProgress(isFinished: true);
        }
    }
}