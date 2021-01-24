using static Dasync.Collections.ParallelForEachExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaImageSearchQuery : IPhilomenaImageSearchQuery
    {
        private readonly PhilomenaApi _api;
        private readonly string _query;
        private readonly string? _apiKey;

        private int _limit = int.MaxValue;
        private SortField? _sortField = null;
        private SortDirection? _sortDirection = null;
        private int? _randomSeed = null;
        private int? _filterId = null;
        private int _maxDownloadThreads = 1;

        private const int _perPage = 50;

        public PhilomenaImageSearchQuery(PhilomenaApi api, string query, string? apiKey = null)
        {
            _api = api;
            _query = query;
            _apiKey = apiKey;
        }

        public async IAsyncEnumerable<IPhilomenaImage> EnumerateResultsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default, IProgress<MetadataDownloadProgressInfo>? progress = null)
        {
            // TODO: Optimize this process and make use of multiple threads
            // TODO: Enumerate using id.gt/id.lt when possible

            // Start on the first page
            int page = 1;

            // Track images processed
            int imagesProcessed = 0;

            // Enumerate images
            ImageSearchModel search;
            do
            {
                // Get the current page of images
                search = await _api.SearchImagesAsync(_query, page, _perPage, _sortField, _sortDirection, _filterId, _apiKey, _randomSeed);

                if (search.Images is null)
                {
                    throw new InvalidOperationException("The search query did not provide a list of images");
                }

                // Update metadata progress if given
                if (progress is not null)
                {
                    if (search.Total is null)
                    {
                        throw new InvalidOperationException("The search query did not provide a total image count");
                    }

                    // Determine how much metadata has been downloaded
                    int metadataDownloaded = imagesProcessed + search.Images.Count;

                    // Cap the total number of images downloaded at the limit
                    int totalImagesToDownload = Math.Min(search.Total.Value, _limit);

                    // Avoid reporting more metadata downloaded than total images to download
                    metadataDownloaded = Math.Min(metadataDownloaded, totalImagesToDownload);

                    // Update progress
                    MetadataDownloadProgressInfo progressInfo = new MetadataDownloadProgressInfo()
                    {
                        Downloaded = metadataDownloaded,
                        Total = totalImagesToDownload
                    };
                    progress.Report(progressInfo);
                }

                // Yield the images
                foreach (ImageModel imageModel in search.Images)
                {
                    if (imagesProcessed >= _limit)
                    {
                        yield break;
                    }

                    IPhilomenaImage image = new PhilomenaImage(imageModel);

                    yield return image;
                    imagesProcessed++;
                }

                // Move to the next page
                page++;
            }
            while (search.Images.Any());  // Stop when there are no more images
        }

        public async Task<IPhilomenaImage> GetFirstAsync()
        {
            IPhilomenaImage? firstImage = await EnumerateResultsAsync().FirstOrDefaultAsync();

            if (firstImage is null) {
                throw new InvalidOperationException("The search query resulted in 0 images");
            }

            return firstImage;
        }

        public IPhilomenaImageSearchQuery Limit(int maxImages)
        {
            _limit = maxImages;

            return this;
        }

        public IPhilomenaImageSearchQuery SortBy(SortField sortField, SortDirection sortDirection)
        {
            _sortField = sortField;
            _sortDirection = sortDirection;

            // Set the random seed if needed
            if (_sortField == SortField.Random)
            {
                _randomSeed = _api.GetRandomSearchSeed();
            }

            return this;
        }

        public IPhilomenaImageSearchQuery WithFilter(int filterId)
        {
            _filterId = filterId;

            return this;
        }

        public IPhilomenaImageSearchQuery WithMaxDownloadThreads(int maxDownloadThreads)
        {
            _maxDownloadThreads = maxDownloadThreads;

            return this;
        }

        public async Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen = false, CancellationToken cancellationToken = default)
        {
            // Run the filtered download with a filter that does nothing
            await DownloadAllAsync(getStreamForImage, leaveOpen, i => i, cancellationToken);
        }

        public async Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen, FilterImagesDelegate filterImages, CancellationToken cancellationToken = default)
        {
            // Filter the images using the custom filter
            IAsyncEnumerable<IPhilomenaImage> imageEnumerable = filterImages(EnumerateResultsAsync(cancellationToken));

            // Download the images using as many threads as configured
            await imageEnumerable.ParallelForEachAsync(async (IPhilomenaImage image) =>
            {
                Stream imageStream = getStreamForImage(image);
                try
                {
                    await image.DownloadToAsync(imageStream, cancellationToken);
                }
                finally
                {
                    if (!leaveOpen)
                    {
                        // Dispose of (and close) the stream
                        imageStream?.Dispose();
                    }
                }
            },
            maxDegreeOfParallelism: _maxDownloadThreads,
            cancellationToken: cancellationToken);
        }

        public async Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, CancellationToken cancellationToken = default)
        {
            // Run the filtered download with a filter that does nothing
            await DownloadAllToFilesAsync(getFileForImage, i => i, cancellationToken);
        }

        public async Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, FilterImagesDelegate filterImages, CancellationToken cancellationToken = default)
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
            filterImages,
            cancellationToken);
        }
    }
}
