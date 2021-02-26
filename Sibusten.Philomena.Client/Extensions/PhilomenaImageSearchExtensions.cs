using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Extensions
{
    public static class PhilomenaImageSearchExtensions
    {
        /// <summary>
        /// Gets a downloader for the image search
        /// </summary>
        /// <param name="search">The image search</param>
        /// <param name="options">Download options</param>
        /// <returns>A downloader for the image search</returns>
        public static IPhilomenaImageDownloader GetDownloader(this IPhilomenaImageSearch search, ImageDownloadOptions? options = null)
        {
            return new PhilomenaImageDownloader(search, options);
        }
    }
}
