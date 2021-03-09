using static Dasync.Collections.ParallelForEachExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Options;
using System.Collections.Concurrent;

namespace Sibusten.Philomena.Client.Images
{
    public class ParallelPhilomenaImageDownloader : IParallelPhilomenaImageDownloader
    {
        private ParallelPhilomenaImageDownloaderOptions _options;

        public ParallelPhilomenaImageDownloader(ParallelPhilomenaImageDownloaderOptions? options = null)
        {
            _options = options ?? new ParallelPhilomenaImageDownloaderOptions();
        }

        public async Task BeginDownload(IAsyncEnumerable<IPhilomenaImage> imagesToDownload, CancellationToken cancellationToken = default, IReadOnlyCollection<IProgress<DownloadProgressInfo>>? progress = null)
        {
            ConcurrentBag<IProgress<DownloadProgressInfo>>? availableProgress = null;

            if (progress is not null)
            {
                // Ensure enough progress entries are provided
                if (progress.Count != _options.MaxDownloadThreads)
                {
                    throw new ArgumentException($"Expected {_options.MaxDownloadThreads} progress entries, but {progress.Count} were provided.", nameof(progress));
                }

                // Copy progress entries to a thread safe structure
                availableProgress = new ConcurrentBag<IProgress<DownloadProgressInfo>>(progress);
            }

            // Download the images using as many threads as configured
            await imagesToDownload.ParallelForEachAsync(async (IPhilomenaImage image) =>
            {
                // Take a progress slot for this image
                IProgress<DownloadProgressInfo>? imageProgress = null;
                availableProgress?.TryTake(out imageProgress);

                // Run configured downloads
                foreach (IPhilomenaDownloader<IPhilomenaImage> downloader in _options.Downloaders)
                {
                    await downloader.Download(image, cancellationToken, imageProgress);
                }

                // Make progress available if one was taken
                if (imageProgress is not null)
                {
                    availableProgress!.Add(imageProgress);
                }
            },
            maxDegreeOfParallelism: _options.MaxDownloadThreads,
            cancellationToken: cancellationToken);
        }
    }
}
