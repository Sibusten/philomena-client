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
            await client
                .GetImageSearch("original_format:svg", o => o
                    .WithSvgMode(SvgMode.Both)
                    .WithMaxImages(5)
                )
                .CreateParallelDownloader(maxDownloadThreads: 1, o => o
                    .WithImageFileDownloader(image => $"ExampleDownloads/DownloadSvgImages/{image.Id}.{image.Format}")
                )
                .BeginDownload();
        }
    }
}
