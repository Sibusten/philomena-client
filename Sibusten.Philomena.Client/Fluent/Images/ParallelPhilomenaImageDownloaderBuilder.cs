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

        public ParallelPhilomenaImageDownloaderBuilder(IAsyncEnumerable<IPhilomenaImage> imagesToDownload)
        {
            _options = new();
            _imagesToDownload = imagesToDownload;
        }

        /// <summary>
        /// The maximum threads to use when downloading images. Defaults to 1.
        /// </summary>
        public ParallelPhilomenaImageDownloaderBuilder WithMaxDownloadThreads(int maxDownloadThreads)
        {
            _options = _options with
            {
                MaxDownloadThreads = maxDownloadThreads
            };
            return this;
        }

        /// <summary>
        /// Adds an image file downloader
        /// </summary>
        /// <param name="getFileForImage">A delegate to get the file for an image</param>
        public ParallelPhilomenaImageDownloaderBuilder WithImageFileDownloader(GetFileForImageDelegate getFileForImage)
        {
            _options = _options with
            {
                Downloaders = _options.Downloaders.Append(
                    new PhilomenaImageFileDownloader(getFileForImage)
                ).ToList()
            };
            return this;
        }

        /// <summary>
        /// Adds an image metadata file downloader
        /// </summary>
        /// <param name="getFileForImage">A delegate to get the file for an image</param>
        public ParallelPhilomenaImageDownloaderBuilder WithImageMetadataFileDownloader(GetFileForImageDelegate getFileForImage)
        {
            _options = _options with
            {
                Downloaders = _options.Downloaders.Append(
                    new PhilomenaImageMetadataFileDownloader(getFileForImage)
                ).ToList()
            };
            return this;
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
    }
}
