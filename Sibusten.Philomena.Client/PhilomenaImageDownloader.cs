using static Dasync.Collections.ParallelForEachExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Utilities;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaImageDownloader : IPhilomenaImageDownloader
    {
        public const int NoImageDownloading = -1;

        private readonly IPhilomenaImageSearch _imageSearch;

        private ImageDownloadOptions _options;

        public PhilomenaImageDownloader(IPhilomenaImageSearch imageSearch, ImageDownloadOptions? options = null)
        {
            _imageSearch = imageSearch;
            _options = options ?? new ImageDownloadOptions();
        }

        public async Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen = false, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null)
        {
            // Run the filtered download with a filter that does nothing
            await DownloadAllAsync(getStreamForImage, leaveOpen, i => true, cancellationToken, progress);
        }

        public async Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen, ShouldDownloadImageDelegate shouldDownloadImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null)
        {
            // Create progress reporting info
            ImageDownloadProgressInfo imageDownloadProgressInfo = new ImageDownloadProgressInfo()
            {
                Downloads = new ParallelImageDownloadProgressInfo[_options.MaxDownloadThreads]
            };
            object progressLock = new object();

            // Blank all image download slots
            for (int i = 0; i < _options.MaxDownloadThreads; i++)
            {
                imageDownloadProgressInfo.Downloads[i].ImageId = NoImageDownloading;
            }

            // Set up metadata download progress hooks
            SyncProgress<MetadataDownloadProgressInfo> metadataProgress = new SyncProgress<MetadataDownloadProgressInfo>(metadataProgressInfo =>
            {
                lock (progressLock)
                {
                    imageDownloadProgressInfo.MetadataDownloaded = metadataProgressInfo.Downloaded;
                    imageDownloadProgressInfo.TotalImages = metadataProgressInfo.Total;
                    progress?.Report(imageDownloadProgressInfo);
                }
            });

            // Begin metadata enumeration
            IAsyncEnumerable<IPhilomenaImage> imageEnumerable = _imageSearch.BeginSearch(cancellationToken, metadataProgress);

            // Download the images using as many threads as configured
            await imageEnumerable.ParallelForEachAsync(async (IPhilomenaImage image) =>
            {
                if (!shouldDownloadImage(image))
                {
                    // Increment images downloaded, since skipped images count toward the limit
                    lock (progressLock)
                    {
                        imageDownloadProgressInfo.ImagesDownloaded++;
                        progress?.Report(imageDownloadProgressInfo);
                    }

                    // Skip this image
                    return;
                }

                // Pick a download slot for this thread
                int downloadSlot;
                lock (progressLock)
                {
                    downloadSlot = Array.FindIndex(imageDownloadProgressInfo.Downloads, d => d.ImageId == NoImageDownloading);

                    if (downloadSlot == -1)
                    {
                        throw new InvalidOperationException("Could not find an open download slot!");
                    }

                    imageDownloadProgressInfo.Downloads[downloadSlot].ImageId = image.Id;
                    imageDownloadProgressInfo.Downloads[downloadSlot].BytesDownloaded = 0;
                    imageDownloadProgressInfo.Downloads[downloadSlot].BytesTotal = null;
                    progress?.Report(imageDownloadProgressInfo);
                }

                // Create a hook for image download progress
                SyncProgress<StreamProgressInfo> downloadProgress = new SyncProgress<StreamProgressInfo>(streamProgressInfo =>
                {
                    // Update the progress of this image download
                    lock (progressLock)
                    {
                        imageDownloadProgressInfo.Downloads[downloadSlot].BytesDownloaded = streamProgressInfo.BytesRead;
                        imageDownloadProgressInfo.Downloads[downloadSlot].BytesTotal = streamProgressInfo.BytesTotal;
                        progress?.Report(imageDownloadProgressInfo);
                    }
                });

                // Begin the download
                Stream imageStream = getStreamForImage(image);
                try
                {
                    await image.DownloadToAsync(imageStream, cancellationToken, downloadProgress);
                }
                finally
                {
                    if (!leaveOpen)
                    {
                        // Dispose of (and close) the stream
                        imageStream?.Dispose();
                    }
                }

                // Update image download count progress and free a download slot
                lock (progressLock)
                {
                    imageDownloadProgressInfo.Downloads[downloadSlot].ImageId = NoImageDownloading;
                    imageDownloadProgressInfo.Downloads[downloadSlot].BytesDownloaded = 0;
                    imageDownloadProgressInfo.Downloads[downloadSlot].BytesTotal = null;
                    imageDownloadProgressInfo.ImagesDownloaded++;
                    progress?.Report(imageDownloadProgressInfo);
                }
            },
            maxDegreeOfParallelism: _options.MaxDownloadThreads,
            cancellationToken: cancellationToken);
        }

        public async Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null)
        {
            // Run the filtered download with a filter that does nothing
            await DownloadAllToFilesAsync(getFileForImage, i => true, cancellationToken, progress);
        }

        public async Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, ShouldDownloadImageDelegate shouldDownloadImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null)
        {
            // Run the stream downloader with a delegate that gets a stream from the image. Make sure the stream is closed.
            await DownloadAllAsync(image =>
            {
                string imageFile = getFileForImage(image);

                // Create directory
                string? imageDirectory = Path.GetDirectoryName(imageFile);
                if (imageDirectory is null)
                {
                    throw new ArgumentException("The file does not have a parent directory", nameof(getFileForImage));
                }
                Directory.CreateDirectory(imageDirectory);

                return File.OpenWrite(imageFile);
            },
            leaveOpen: false,
            shouldDownloadImage,
            cancellationToken,
            progress);
        }
    }
}
