using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Images.Downloaders;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Fluent.Images
{
    public class ParallelPhilomenaImageDownloaderBuilder
    {
        private ParallelPhilomenaImageDownloaderOptions _options;
        private IAsyncEnumerable<IPhilomenaImage> _imagesToDownload;

        public ParallelPhilomenaImageDownloaderBuilder(IAsyncEnumerable<IPhilomenaImage> imagesToDownload) : this(imagesToDownload, options: new()) { }

        private ParallelPhilomenaImageDownloaderBuilder(IAsyncEnumerable<IPhilomenaImage> imagesToDownload, ParallelPhilomenaImageDownloaderOptions options)
        {
            _options = options;
            _imagesToDownload = imagesToDownload;
        }

        /// <summary>
        /// The maximum threads to use when downloading images. Defaults to 1.
        /// </summary>
        public ParallelPhilomenaImageDownloaderBuilder WithMaxDownloadThreads(int maxDownloadThreads)
        {
            return new(_imagesToDownload, _options with
            {
                MaxDownloadThreads = maxDownloadThreads
            });
        }

        /// <summary>
        /// Adds an image file downloader
        /// </summary>
        /// <param name="getFileForImage">A delegate to get the file for an image</param>
        public WithDownloader WithImageFileDownloader(GetFileForImageDelegate getFileForImage)
        {
            return new(_imagesToDownload, _options with
            {
                Downloaders = _options.Downloaders.Append(
                    new PhilomenaImageFileDownloader(getFileForImage)
                ).ToList()
            });
        }

        /// <summary>
        /// Adds an image metadata file downloader
        /// </summary>
        /// <param name="getFileForImage">A delegate to get the file for an image</param>
        public WithDownloader WithImageMetadataFileDownloader(GetFileForImageDelegate getFileForImage)
        {
            return new(_imagesToDownload, _options with
            {
                Downloaders = _options.Downloaders.Append(
                    new PhilomenaImageMetadataFileDownloader(getFileForImage)
                ).ToList()
            });
        }

        /// <summary>
        /// Begins the parallel download
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="progress">A list of progress reporters. There must be one progress reporter given for each download slot</param>
        public async Task BeginDownload(CancellationToken cancellationToken = default, IReadOnlyCollection<IProgress<DownloadProgressInfo>>? progress = null)
        {
            ParallelPhilomenaImageDownloader downloader = new ParallelPhilomenaImageDownloader(_options);
            await downloader.BeginDownload(_imagesToDownload, cancellationToken, progress);
        }

        public class WithDownloader : ParallelPhilomenaImageDownloaderBuilder
        {
            public WithDownloader(IAsyncEnumerable<IPhilomenaImage> imagesToDownload, ParallelPhilomenaImageDownloaderOptions options) : base(imagesToDownload, options) { }

            /// <summary>
            /// Adds a condition to the last downloader added
            /// </summary>
            /// <param name="shouldDownloadImage">A delegate that returns true if an image should be downloaded</param>
            public WithDownloader WithDownloadCondition(ShouldDownloadImageDelegate shouldDownloadImage)
            {
                // Wrap the last downloader in a conditional downloader
                var lastDownloader = _options.Downloaders.Last();
                var conditionalDownloader = new ConditionalImageDownloader(shouldDownloadImage, lastDownloader);
                var downloadersExceptLast = _options.Downloaders.Take(_options.Downloaders.Count - 1);

                return new(_imagesToDownload, _options with
                {
                    Downloaders = downloadersExceptLast.Append(conditionalDownloader).ToList()
                });
            }
        }
    }
}
