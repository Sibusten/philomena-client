using System;
using Philomena.Client.Api;

namespace Philomena.Client
{
    public class PhilomenaClient : IPhilomenaClient
    {
        private PhilomenaApi _api;

        public string? ApiKey { get; set; } = null;

        public PhilomenaClient(string baseUrl)
        {
            _api = new PhilomenaApi(baseUrl);
        }

        public ISearchQuery Search(string query)
        {
            return new SearchQuery(_api, query, ApiKey);
        }
    }
}
