using System;
using System.Collections.Generic;
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

        private const int _perPage = 50;

        public SearchQuery(PhilomenaApi api, string query, string? apiKey = null)
        {
            _api = api;
            _query = query;
            _apiKey = apiKey;
        }

        public async IAsyncEnumerable<IImage> EnumerateResultsAsync()
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

                    yield return new Image(imageModel);
                    imagesProcessed++;
                }

                // Move to the next page
                page++;
            }
            while (search.Images.Any());  // Stop when there are no more images
        }

        public async Task<IImage> GetFirstAsync()
        {
            // Get the first page of images
            ImageSearchModel search = await _api.SearchImagesAsync(_query, page: 1, perPage: 1, _sortField, _sortDirection, _filterId, _apiKey, _randomSeed);

            if (search.Images is null)
            {
                throw new InvalidOperationException("The search query did not provide a list of images");
            }

            if (!search.Images.Any())
            {
                throw new InvalidOperationException("The search query resulted in 0 images");
            }

            return new Image(search.Images.First());
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
    }
}
