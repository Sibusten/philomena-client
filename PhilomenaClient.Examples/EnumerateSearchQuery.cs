using System.IO;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Examples
{
    public class EnumerateSearchQuery : IExample
    {
        public string Description => "Enumerate a search query and save images to files";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");
            ISearchQuery query = client.Search("fluttershy").Limit(10);

            // Using download all method
            await query.DownloadAllAsync(image => new FileInfo($"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}"));

            // Using delegate methods and a custom condition to skip images already downloaded
            // Note that using direct query filtering methods are preferred since they will provide better performance. Don't use the custom condition for conditions like image score.
            query.AddCustomCondition(ShouldDownloadImage);
            await query.DownloadAllAsync(GetFileForImage);

            // Explicitly looping over each image and saving
            await foreach(IPhilomenaImage image in query.EnumerateResultsAsync())
            {
                string filename = $"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}";
                FileInfo file = new FileInfo(filename);

                await image.DownloadToFileAsync(file);
            }
        }

        private FileInfo GetFileForImage(IPhilomenaImage image)
        {
            // A custom file naming scheme could be used here to generate file names
            return new FileInfo($"ExampleDownloads/EnumerateSearchQuery/{image.Model.Id}.{image.Model.Format}");
        }

        private bool ShouldDownloadImage(IPhilomenaImage image)
        {
            // Exclude images already downloaded.
            // This is a simple way of determining this that will fail if file names are not the same each download. A database could be used here instead.
            FileInfo imageFile = GetFileForImage(image);
            return !imageFile.Exists;
        }
    }
}
