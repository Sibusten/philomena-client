using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private List<ShouldProcessImageDelegate> _customConditions = new();

        private const int _perPage = 50;

        public SearchQuery(PhilomenaApi api, string query, string? apiKey = null)
        {
            _api = api;
            _query = query;
            _apiKey = apiKey;
        }

        /// <summary>
        /// Evaluates the custom conditions and determines whether an image should be processed
        /// </summary>
        /// <param name="image">The image being considered</param>
        /// <returns>True if the image should be processed</returns>
        private bool CustomConditionsPass(IPhilomenaImage image)
        {
            foreach (ShouldProcessImageDelegate shouldProcessImage in _customConditions)
            {
                if (!shouldProcessImage(image))
                {
                    return false;
                }
            }

            return true;
        }

        public async IAsyncEnumerable<IPhilomenaImage> EnumerateResultsAsync()
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

                    // Check custom conditions
                    if (!CustomConditionsPass(image))
                    {
                        // Skip this image
                        continue;
                    }

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

        public ISearchQuery AddCustomCondition(ShouldProcessImageDelegate shouldProcessImage)
        {
            _customConditions.Add(shouldProcessImage);

            return this;
        }

        public async Task DownloadAllAsync(GetFileForImageDelegate getFileForImage)
        {
            await foreach(IPhilomenaImage image in EnumerateResultsAsync())
            {
                FileInfo imageFile = getFileForImage(image);
                await image.DownloadToFileAsync(imageFile);
            }
        }
    }
}
