using static Dasync.Collections.ParallelForEachExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Options;
using System.Collections.Concurrent;
using Flurl.Http;
using Sibusten.Philomena.Client.Logging;
using Microsoft.Extensions.Logging;

namespace Sibusten.Philomena.Client.Images.Downloaders
{
    public class ParallelPhilomenaImageDownloader : IParallelPhilomenaImageDownloader
    {
        private ILogger _logger;

        private ParallelPhilomenaImageDownloaderOptions _options;

        public ParallelPhilomenaImageDownloader(ParallelPhilomenaImageDownloaderOptions? options = null)
        {
            _logger = Logger.Factory.CreateLogger(GetType());

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
            await imagesToDownload.ParallelForEachAsync
            (
                async (image) => await DownloadImage(image, availableProgress, cancellationToken),
                maxDegreeOfParallelism: _options.MaxDownloadThreads,
                cancellationToken: cancellationToken
            );
        }

        private async Task DownloadImage(IPhilomenaImage image, ConcurrentBag<IProgress<DownloadProgressInfo>>? availableProgress, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Downloading image {ImageId}", image.Id);

            // Take a progress slot for this image
            IProgress<DownloadProgressInfo>? imageProgress = null;
            availableProgress?.TryTake(out imageProgress);

            // Run configured downloads
            foreach (IPhilomenaDownloader<IPhilomenaImage> downloader in _options.Downloaders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await downloader.Download(image, cancellationToken, imageProgress);
            }

            // Make progress available if one was taken
            if (imageProgress is not null)
            {
                availableProgress!.Add(imageProgress);
            }
        }
    }
}
