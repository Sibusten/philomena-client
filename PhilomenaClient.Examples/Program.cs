using System;
using System.Threading.Tasks;
using Philomena.Client.Api;
using Philomena.Client.Api.Models;

namespace Philomena.Client.Examples
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            PhilomenaApi api = new PhilomenaApi("https://derpibooru.org");

            ImageSearchModel searchResults = await api.SearchImages("fluttershy");
            Console.WriteLine($"Found {searchResults.Total} images");

            return;


            PhilomenaClient client = new PhilomenaClient();

            ISearchQuery query = client
                .Search("fluttershy")
                .SortBy(SortField.ImageId, SortDirection.Descending);

            await foreach (IImage image in query.EnumerateResultsAsync())
            {
                await image.DownloadToFileAsync("filename");
            }
        }
    }
}
