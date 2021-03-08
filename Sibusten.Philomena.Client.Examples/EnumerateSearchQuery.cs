using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Fluent.Images;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            PhilomenaImageSearchBuilder search = client.SearchImages("fluttershy").WithMaxImages(5);

            Console.WriteLine("Download query simple");

            // Using download all method
            await search
                .BeginSearch()
                .CreateParallelDownloader()
                    .WithImageFileDownloader(image => $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}")
                .BeginDownload();

            Console.WriteLine("Download query with delegates, skipping existing images");

            // Use Linq to skip images already downloaded
            // Note that filters in the search query itself are preferred since they will provide better performance. Don't use Linq filtering for conditions like image score.
            await search
                .BeginSearch()
                .Where(SkipImagesAlreadyDownloaded)
                .CreateParallelDownloader()
                    .WithImageFileDownloader(GetFileForImage)
                .BeginDownload();

            Console.WriteLine("Downloading images explicitly");

            // Explicitly looping over each image and saving
            await foreach (IPhilomenaImage image in search.BeginSearch())
            {
                string filename = $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}";

                await image.DownloadToFile(filename);
            }

            Console.WriteLine("Downloading with multiple threads and progress updates");

            // Downloading with multiple threads and progress updates
            // Also skips downloaded images like before
            SyncProgress<ImageSearchProgressInfo> progress = new SyncProgress<ImageSearchProgressInfo>(DownloadProgressUpdate);
            await client
                .SearchImages("fluttershy")
                    .WithMaxImages(100)
                .BeginSearch(searchProgress: progress)
                .Where(SkipImagesAlreadyDownloaded)
                .CreateParallelDownloader()
                    .WithMaxDownloadThreads(8)
                    .WithImageFileDownloader(GetFileForImage)
                    .WithImageMetadataFileDownloader(GetMetadataFileForImage)
                .BeginDownload();
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

        private void DownloadProgressUpdate(ImageSearchProgressInfo imageSearchProgressInfo)
        {
            // Simple progress updating to console
            double percentDownloaded = (double)imageSearchProgressInfo.Processed / (double)imageSearchProgressInfo.Total * 100.0;
            Console.WriteLine($"{imageSearchProgressInfo.Processed}/{imageSearchProgressInfo.Total} ({percentDownloaded:00.0}%)");
        }
    }
}
