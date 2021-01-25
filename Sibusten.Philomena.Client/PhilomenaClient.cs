﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;

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

        public IPhilomenaImageSearchQuery Search(string query)
        {
            return new PhilomenaImageSearchQuery(_api, query, ApiKey);
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