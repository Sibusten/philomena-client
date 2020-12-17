using System;
using System.Linq;
using System.Threading.Tasks;
using Philomena.Client.Api;
using Philomena.Client.Api.Models;

namespace Philomena.Client.Examples.Api
{
    public class SearchImages : IExample
    {
        public string Description => "Search for images";

        private void PrintImageInfo(ImageModel image)
        {
            Console.WriteLine($"ID: {image.Id}");
            Console.WriteLine($"Score: {image.Score}");
            Console.WriteLine($"Description: {image.Description}");
            Console.WriteLine($"Full size image URL: {image.Representations.Full}");
            Console.WriteLine($"Tags: {string.Join(", ", image.Tags)}");
            Console.WriteLine($"Source: {image.SourceUrl}");
        }

        private async Task SearchDefault(PhilomenaApi api, string searchQuery)
        {
            Console.WriteLine("Using default options (sort by ID, descending)");
            ImageSearchModel searchResults = await api.SearchImages(searchQuery);

            Console.WriteLine($"Found {searchResults.Total} images");
            Console.WriteLine();

            ImageModel newestImage = searchResults.Images.First();
            Console.WriteLine("Newest image:");
            PrintImageInfo(newestImage);
            Console.WriteLine();
        }

        private async Task SearchHighestRated(PhilomenaApi api, string searchQuery)
        {
            Console.WriteLine("Searching for highest rated image");
            ImageSearchModel searchResults = await api.SearchImages(searchQuery, sortField: SortField.Score);

            ImageModel highestScoringImage = searchResults.Images.First();
            Console.WriteLine("Highest scoring image:");
            PrintImageInfo(highestScoringImage);
            Console.WriteLine();
        }

        private async Task SearchRandom(PhilomenaApi api, string searchQuery)
        {
            Console.WriteLine("Searching for a random image");

            // When searching randomly, get a seed and use it in all pages for that search.
            int randomSeed = api.GetRandomSearchSeed();
            Console.WriteLine($"Random seed: {randomSeed}");

            ImageSearchModel searchResults = await api.SearchImages(searchQuery, sortField: SortField.Random, randomSeed: randomSeed);

            ImageModel randomImage = searchResults.Images.First();
            Console.WriteLine("Random image:");
            PrintImageInfo(randomImage);
            Console.WriteLine();

            // Use the same seed for later pages
            searchResults = await api.SearchImages(searchQuery, page: 2, sortField: SortField.Random, randomSeed: randomSeed);

            randomImage = searchResults.Images.First();
            Console.WriteLine("Random image page 2:");
            PrintImageInfo(randomImage);
            Console.WriteLine();
        }

        public async Task RunExample()
        {
            Console.WriteLine("Creating API client");
            PhilomenaApi api = new PhilomenaApi("https://derpibooru.org");

            string searchQuery = "safe, fluttershy, rainbow dash, (transparent background || white background)";
            Console.WriteLine($"Using search query: '{searchQuery}'");

            // Search with default options
            await SearchDefault(api, searchQuery);

            // Search for highest rated image
            await SearchHighestRated(api, searchQuery);

            // Search for random images
            await SearchRandom(api, searchQuery);
        }
    }
}
