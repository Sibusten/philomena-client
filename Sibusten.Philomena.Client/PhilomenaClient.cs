using System;
using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaClient : IPhilomenaClient
    {
        private PhilomenaApi _api;

        public string? ApiKey { get; set; } = null;

        public PhilomenaClient(string baseUrl)
        {
            _api = new PhilomenaApi(baseUrl);
        }

        public IPhilomenaImageSearch Search(string query, ImageSearchOptions? options = null)
        {
            // Create options if not provided
            options = options ?? new ImageSearchOptions();

            // Set API Key if not overridden
            if (options.ApiKey is null)
            {
                options = options with
                {
                    ApiKey = ApiKey
                };
            }

            return new PagedPhilomenaImageSearch(_api, query, options);
        }

        public async Task<TagModel> GetTagById(int tagId)
        {
            string tagQuery = $"id:{tagId}";

            TagSearchModel tagSearch = await _api.SearchTagsAsync(tagQuery, 1, 1);

            if (tagSearch.Tags is null)
            {
                throw new InvalidOperationException("The search query did not provide a list of tags");
            }

            if (!tagSearch.Tags.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(tagId), tagId, "A tag with this ID was not found");
            }

            return tagSearch.Tags.First();
        }
    }
}
