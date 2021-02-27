using System.Collections.Generic;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Images.Downloaders;
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
            IPhilomenaImageSearch search = client.Search("original_format:svg", new()
            {
                SvgMode = SvgMode.Both,
                MaxImages = 5
            });
            ParallelPhilomenaImageDownloader downloader = new ParallelPhilomenaImageDownloader(new()
            {
                Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>
                {
                    new PhilomenaImageFileDownloader(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}")
                }
            });
            await downloader.Download(search.BeginSearch());

            // Alternatively, with fluent extensions
            await client
                .Search("original_format:svg", new()
                {
                    SvgMode = SvgMode.Both,
                    MaxImages = 5
                })
                .DownloadAllParallel(new()
                {
                    Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>
                    {
                        new PhilomenaImageFileDownloader(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}")
                    }
                });
        }
    }
}
