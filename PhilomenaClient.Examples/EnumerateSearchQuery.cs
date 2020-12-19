using System.IO;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            ISearchQuery query = client.Search("fluttershy").Limit(100);

            await foreach(IImage image in query.EnumerateResultsAsync())
            {
                string filename = $"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}";
                FileInfo file = new FileInfo(filename);

                await image.DownloadToFileAsync(file);
            }
        }
    }
}
