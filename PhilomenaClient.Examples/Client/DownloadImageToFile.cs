using System.IO;
using System.Threading.Tasks;

namespace Philomena.Client.Examples.Client
{
    public class DownloadImageToFile : IExample
    {
        public string Description => "Download an image to a file";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient();
            IImage image = await client.Search("fluttershy").GetFirstAsync();

            string filename = $"{image.Model.Id}.{image.Model.Format}";
            FileInfo file = new FileInfo(filename);

            await image.DownloadToFileAsync(file);
        }
    }
}
