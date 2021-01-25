using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            IPhilomenaImageSearchQuery query = client.Search("fluttershy").Limit(5);

            Console.WriteLine("Download query simple");

            // Using download all method
            await query.DownloadAllToFilesAsync(image => $"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}");

            Console.WriteLine("Download query with delegates, skipping existing images");

            // Using a delegate method and a custom filter to skip images already downloaded
            // Note that using direct query filtering methods are preferred since they will provide better performance. Don't use the custom filter for conditions like image score.
            await query.DownloadAllToFilesAsync(GetFileForImage, SkipImagesAlreadyDownloaded);

            Console.WriteLine("Downloading images explicitly");

            // Explicitly looping over each image and saving
            await foreach (IPhilomenaImage image in query.EnumerateResultsAsync())
            {
                string filename = $"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}";

                await image.DownloadToFileAsync(filename);
            }

            Console.WriteLine("Downloading with multiple threads and progress updates");

            // Downloading with multiple threads and progress updates
            // Also skips downloaded images like before
            SyncProgress<ImageDownloadProgressInfo> progress = new SyncProgress<ImageDownloadProgressInfo>(DownloadProgressUpdate);
            await client
                .Search("fluttershy")
                .Limit(100)
                .WithMaxDownloadThreads(8)
                .DownloadAllToFilesAsync(GetFileForImage, SkipImagesAlreadyDownloaded, progress: progress);
        }

        private string GetFileForImage(IPhilomenaImage image)
        {
            // A custom file naming scheme could be used here to generate file names
            return $"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}";
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

        private int _lastImageProgress = 0;

        private void DownloadProgressUpdate(ImageDownloadProgressInfo imageDownloadProgressInfo)
        {
            // Only write to console when images are downloaded
            // NOTE: If more information is being reported, consider using a separate polling thread to update on an interval and having the progress report function only update a variable. This prevents slowing the download.
            if (imageDownloadProgressInfo.ImagesDownloaded == _lastImageProgress)
            {
                return;
            }
            _lastImageProgress = imageDownloadProgressInfo.ImagesDownloaded;

            // Simple progress updating to console. Progress for each download thread is also available, but not used here.
            double percentDownloaded = (double)imageDownloadProgressInfo.ImagesDownloaded / (double)imageDownloadProgressInfo.TotalImages * 100.0;
            Console.WriteLine($"{imageDownloadProgressInfo.ImagesDownloaded}/{imageDownloadProgressInfo.TotalImages} ({percentDownloaded:00.0}%)");
        }
    }
}
