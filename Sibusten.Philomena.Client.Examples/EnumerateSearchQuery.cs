using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Images.Downloaders;
using Sibusten.Philomena.Client.Options;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            IPhilomenaImageSearch search = client.Search("fluttershy", new ImageSearchOptions
            {
                MaxImages = 5
            });

            Console.WriteLine("Download query simple");

            // Using download all method
            await search.DownloadAllParallel(new()
            {
                Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>
                {
                    new PhilomenaImageFileDownloader(image => $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}")
                }
            });

            Console.WriteLine("Download query with delegates, skipping existing images");

            // Use Linq to skip images already downloaded
            // Note that filters in the search query itself are preferred since they will provide better performance. Don't use Linq filtering for conditions like image score.
            await search
                .BeginSearch()
                .Where(SkipImagesAlreadyDownloaded)
                .DownloadAllParallel(new()
                {
                    Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>
                    {
                        new PhilomenaImageFileDownloader(GetFileForImage)
                    }
                });

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
                .Search("fluttershy", new()
                {
                    MaxImages = 100
                })
                .BeginSearch(searchProgress: progress)
                .Where(SkipImagesAlreadyDownloaded)
                .DownloadAllParallel(new()
                {
                    MaxDownloadThreads = 8,
                    Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>
                    {
                        new PhilomenaImageFileDownloader(GetFileForImage)
                    }
                });
        }

        private string GetFileForImage(IPhilomenaImage image)
        {
            // A custom file naming scheme could be used here to generate file names
            return $"ExampleDownloads/EnumerateSearchQuery/{image.Id}.{image.Format}";
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
