using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dasync.Collections;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Client
{
    public class SearchQuery : ISearchQuery
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

        public SearchQuery(PhilomenaApi api, string query, string? apiKey = null)
        {
            _api = api;
            _query = query;
            _apiKey = apiKey;
        }

        public async IAsyncEnumerable<IPhilomenaImage> EnumerateResultsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
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
            await foreach (IPhilomenaImage image in EnumerateResultsAsync())
            {
                return image;
            }

            throw new InvalidOperationException("The search query resulted in 0 images");
        }

        public ISearchQuery Limit(int maxImages)
        {
            _limit = maxImages;

            return this;
        }

        public ISearchQuery SortBy(SortField sortField, SortDirection sortDirection)
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

        public ISearchQuery WithFilter(int filterId)
        {
            _filterId = filterId;

            return this;
        }

        public ISearchQuery WithMaxDownloadThreads(int maxDownloadThreads)
        {
            _maxDownloadThreads = maxDownloadThreads;

            return this;
        }

        public async Task DownloadAllAsync(GetFileForImageDelegate getFileForImage, CancellationToken cancellationToken = default)
        {
            // Run the filtered download with a filter that does nothing
            await DownloadAllAsync(getFileForImage, i => i, cancellationToken);
        }

        public async Task DownloadAllAsync(GetFileForImageDelegate getFileForImage, FilterImagesDelegate filterImages, CancellationToken cancellationToken = default)
        {
            // Filter the images using the custom filter
            IAsyncEnumerable<IPhilomenaImage> imageEnumerable = filterImages(EnumerateResultsAsync(cancellationToken));

            // Download the images using as many threads as configured
            await imageEnumerable.ParallelForEachAsync(async (IPhilomenaImage image) =>
            {
                FileInfo imageFile = getFileForImage(image);
                await image.DownloadToFileAsync(imageFile, cancellationToken);
            },
            maxDegreeOfParallelism: _maxDownloadThreads,
            cancellationToken: cancellationToken);
        }
    }
}
