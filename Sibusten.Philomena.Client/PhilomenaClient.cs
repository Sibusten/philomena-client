﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Fluent.Images;
using Sibusten.Philomena.Client.Logging;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaClient : IPhilomenaClient
    {
        private ILogger _logger;

        private PhilomenaApi _api;

        public string? ApiKey { get; set; } = null;

        public PhilomenaClient(string baseUrl)
        {
            _logger = Logger.Factory.CreateLogger(GetType());

            _api = new PhilomenaApi(baseUrl);
        }

        public PhilomenaImageSearchBuilder SearchImages(string query)
        {
            return new PhilomenaImageSearchBuilder(_api, query);
        }

        public async Task<TagModel> GetTagById(int tagId)
        {
            string tagQuery = $"id:{tagId}";

            _logger.LogDebug("Searching for tags: '{Query}'", tagQuery);

            TagSearchModel tagSearch = await _api.SearchTagsAsync(tagQuery, page: 1, perPage: 1);

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
