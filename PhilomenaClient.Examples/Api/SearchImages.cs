using System;
using System.Linq;
using System.Threading.Tasks;
using Philomena.Client.Api;
using Philomena.Client.Api.Models;

namespace Philomena.Client.Examples.Api
{
    public class SearchImages : IExample
    {
        public string Description => "Searches for images";

        private void PrintImageInfo(ImageModel image)
        {
            Console.WriteLine($"ID: {image.Id}");
            Console.WriteLine($"Score: {image.Score}");
            Console.WriteLine($"Description: {image.Description}");
            Console.WriteLine($"Full size image URL: {image.Representations.Full}");
            Console.WriteLine($"Tags: {string.Join(", ", image.Tags)}");
            Console.WriteLine($"Source: {image.SourceUrl}");
        }

        public async Task RunExample()
        {
            Console.WriteLine("Creating API client");
            PhilomenaApi api = new PhilomenaApi("https://derpibooru.org");

            string searchQuery = "safe, fluttershy, rainbow dash, (transparent background || white background)";

            Console.WriteLine($"Searching for '{searchQuery}'");
            ImageSearchModel searchResults = await api.SearchImages(searchQuery);

            Console.WriteLine($"Found {searchResults.Total} images");
            Console.WriteLine();

            ImageModel newestImage = searchResults.Images.First();
            Console.WriteLine("Newest image:");
            PrintImageInfo(newestImage);

            // TODO: Console.WriteLine("Searching for highest rated image")
        }
    }
}
