using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Philomena.Client.Api.Models;
using System;

namespace Philomena.Client.Api
{
    public class PhilomenaApi : IPhilomenaApi
    {
        private const string _filterIdParam = "filter_id";
        private const string _apiKeyParam = "key";
        private const string _pageParam = "page";
        private const string _perPageParam = "per_page";
        private const string _queryParam = "q";
        private const string _sortDirectionParam = "sd";
        private const string _sortFieldParam = "sf";

        private const string _userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

        private string _baseUrl;
        private IFlurlRequest _apiRequest => _baseUrl.AppendPathSegment("api/v1/json").WithHeader("User-Agent", _userAgent);

        private string GetSortDirectionParamValue(SortDirection sortDirection)
        {
            return sortDirection switch
            {
                SortDirection.Descending => "desc",
                SortDirection.Ascending => "asc",

                _ => throw new ArgumentOutOfRangeException(nameof(sortDirection))
            };
        }

        private string GetSortFieldParamValue(SortField sortField)
        {
            return sortField switch
            {
                SortField.ImageId => "id",
                SortField.LastModificationDate => "updated_at",
                SortField.InitialPostDate => "first_seen_at",
                SortField.AspectRatio => "aspect_ratio",
                SortField.FaveCount => "faves",
                SortField.Upvotes => "upvotes",
                SortField.Downvotes => "downvotes",
                SortField.Score => "score",
                SortField.WilsonScore => "wilson_score",
                SortField.Relevance => "_score",
                SortField.Width => "width",
                SortField.Height => "height",
                SortField.Comments => "comment_count",
                SortField.TagCount => "tag_count",
                SortField.Pixels => "pixels",
                SortField.FileSize => "size",
                SortField.Duration => "duration",
                SortField.Random => "random",

                _ => throw new ArgumentOutOfRangeException(nameof(sortField))
            };
        }

        public PhilomenaApi(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<TagModel> GetTag(string tagSlug)
        {
            TagResponseModel tagRoot = await _apiRequest
                .AppendPathSegment("tags")
                .AppendPathSegment(tagSlug)
                .GetJsonAsync<TagResponseModel>();

            if (tagRoot.Tag is null)
            {
                throw new ArgumentException($"Tag {tagSlug} does not exist");
            }

            return tagRoot.Tag;
        }

        public async Task<ImageSearchModel> SearchImages(string query, int? page = null, int? perPage = null, SortField? sortField = null, SortDirection? sortDirection = null, int? filterId = null, string? apiKey = null)
        {
            string? sortFieldParamValue = (sortField is null) ? null : GetSortFieldParamValue(sortField.Value);
            string? sortDirectionParamValue = (sortDirection is null) ? null : GetSortDirectionParamValue(sortDirection.Value);

            return await _apiRequest
                .AppendPathSegment("search/images")
                .SetQueryParam(_queryParam, query)
                .SetQueryParam(_pageParam, page)
                .SetQueryParam(_perPageParam, perPage)
                .SetQueryParam(_sortFieldParam, sortFieldParamValue)  // TODO: random field adds a random value after the field
                .SetQueryParam(_sortDirectionParam, sortDirectionParamValue)
                .SetQueryParam(_filterIdParam, filterId)
                .SetQueryParam(_apiKeyParam, apiKey)
                .GetJsonAsync<ImageSearchModel>();
        }
    }
}
