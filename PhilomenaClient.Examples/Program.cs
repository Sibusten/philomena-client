using System;
using System.Threading.Tasks;
using Philomena.Client;

namespace Philomena.Client.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
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
