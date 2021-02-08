using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Examples
{
    public class DownloadSvgImages : IExample
    {
        public string Description => "Download original SVG images";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");

            // Download both SVG sources and rasters
            IPhilomenaImageSearchQuery query = client.Search("original_format:svg").WithSvgMode(SvgMode.Both).Limit(5).WithMaxDownloadThreads(4);
            await query.DownloadAllToFilesAsync(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}");
        }
    }
}
