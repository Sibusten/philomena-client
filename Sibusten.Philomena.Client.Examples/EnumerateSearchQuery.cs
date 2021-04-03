using System.IO;
using System.Threading.Tasks;
using Serilog;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Images.Downloaders;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            IPhilomenaImageSearch search = client.GetImageSearch("fluttershy", o => o
                .WithMaxImages(5)
            );

            Log.Information("Download query simple");

            // Using download all method
            await search
                .CreateParallelDownloader(maxDownloadThreads: 8, o => o
                    .WithImageFileDownloader(image => $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}")
                )
                .BeginDownload();

            Log.Information("Download query with delegates, skipping existing images");

            // Use a conditional downloader to skip images already downloaded
            // Note that filters in the search query itself are preferred since they will provide better performance. Don't use conditional downloaders for conditions like image score.
            await search
                .CreateParallelDownloader(maxDownloadThreads: 8, o => o
                    .WithConditionalDownloader(SkipImagesAlreadyDownloaded, o => o
                        .WithImageFileDownloader(GetFileForImage)
                    )
                )
                .BeginDownload();

            Log.Information("Downloading images explicitly");

            // Explicitly looping over each image and saving
            await foreach (IPhilomenaImage image in search.BeginSearch())
            {
                string filename = $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}";

                await image.DownloadToFile(filename);
            }

            Log.Information("Downloading with multiple threads and progress updates");

            // Downloading with multiple threads and progress updates
            // Also skips downloaded images like before
            SyncProgress<PhilomenaImageSearchDownloadProgressInfo> progress = new SyncProgress<PhilomenaImageSearchDownloadProgressInfo>(DownloadProgressUpdate);
            await client
                .GetImageSearch("fluttershy", o => o
                    .WithMaxImages(100)
                )
                .CreateParallelDownloader(maxDownloadThreads: 8, o => o
                    .WithConditionalDownloader(SkipImagesAlreadyDownloaded, o => o
                        .WithImageFileDownloader(GetFileForImage)
                        .WithImageMetadataFileDownloader(GetMetadataFileForImage)
                    )
                )
                .BeginDownload(searchDownloadProgress: progress);
        }

        private string GetFileForImage(IPhilomenaImage image)
        {
            // A custom file naming scheme could be used here to generate file names
            return $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}";
        }

        private string GetMetadataFileForImage(IPhilomenaImage image)
        {
            // A custom file naming scheme could be used here to generate file names
            return $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.json";
        }

        private bool ImageExists(IPhilomenaImage image)
        {
            // Exclude images already downloaded.
            // This is a simple way of determining this that will fail if file names are not the same each download. A database could be used here instead.
            string imageFile = GetFileForImage(image);
            return File.Exists(imageFile);
        }

        private bool SkipImagesAlreadyDownloaded(IPhilomenaImage image)
        {
            return !ImageExists(image);
        }

        private void DownloadProgressUpdate(PhilomenaImageSearchDownloadProgressInfo imageSearchProgressInfo)
        {
            // Simple progress updating to console
            double percentDownloaded = (double)imageSearchProgressInfo.ImagesDownloaded / (double)imageSearchProgressInfo.ImagesTotal * 100.0;
            Log.Information("Progress update: {ImagesDownloaded}/{ImagesTotal} ({PercentDownloaded:#0.0}%)", imageSearchProgressInfo.ImagesDownloaded, imageSearchProgressInfo.ImagesTotal, percentDownloaded);
        }
    }
}
