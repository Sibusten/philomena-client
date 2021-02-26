using System.Threading.Tasks;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Examples
{
    public class DownloadSvgImages : IExample
    {
        public string Description => "Download original SVG images";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");

            // Download both SVG sources and rasters
            IPhilomenaImageSearch search = client.Search("original_format:svg", new ImageSearchOptions
            {
                SvgMode = SvgMode.Both,
                MaxImages = 5
            });
            IPhilomenaImageDownloader downloader = new PhilomenaImageDownloader(search);
            await downloader.DownloadAllToFilesAsync(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}");

            // Alternatively, with fluent extensions
            await client
                .Search("original_format:svg", new ImageSearchOptions
                {
                    SvgMode = SvgMode.Both,
                    MaxImages = 5
                })
                .GetDownloader()
                .DownloadAllToFilesAsync(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}");
        }
    }
}
