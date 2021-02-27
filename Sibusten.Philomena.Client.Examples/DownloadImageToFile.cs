using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;

namespace Sibusten.Philomena.Client.Examples
{
    public class DownloadImageToFile : IExample
    {
        public string Description => "Download an image to a file";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            IPhilomenaImage image = await client.Search("fluttershy").BeginSearch().FirstAsync();

            string filename = $"ExampleDownloads/DownloadImageToFile/{image.Id}.{image.Format}";

            await image.DownloadToFile(filename);
        }
    }
}
