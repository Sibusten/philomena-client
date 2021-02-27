using System.Collections.Generic;
using System.Threading.Tasks;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Extensions
{
    public static class PhilomenaImageSearchExtensions
    {
        /// <summary>
        /// Downloads images from a search in parallel
        /// </summary>
        /// <param name="search">The image search</param>
        /// <param name="options">Download options</param>
        public static async Task DownloadAllParallel(this IPhilomenaImageSearch search, ParallelPhilomenaImageDownloaderOptions? options = null)
        {
            await search.BeginSearch().DownloadAllParallel(options);
        }

        /// <summary>
        /// Downloads images in parallel
        /// </summary>
        /// <param name="images">The images to download</param>
        /// <param name="options">Download options</param>
        public static async Task DownloadAllParallel(this IAsyncEnumerable<IPhilomenaImage> images, ParallelPhilomenaImageDownloaderOptions? options = null)
        {
            ParallelPhilomenaImageDownloader downloader = new ParallelPhilomenaImageDownloader(options);
            await downloader.Download(images);
        }
    }
}
