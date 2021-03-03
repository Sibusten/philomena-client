using System.Collections.Generic;
using Sibusten.Philomena.Client.Fluent.Images;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Extensions
{
    public delegate ParallelPhilomenaImageDownloaderOptions ConfigureParallelPhilomenaImageDownloaderOptionsDelegate(ParallelPhilomenaImageDownloaderOptions options);

    public static class PhilomenaImageSearchExtensions
    {
        /// <summary>
        /// Creates a parallel downloader for images
        /// </summary>
        /// <param name="images">The images to download</param>
        public static ParallelPhilomenaImageDownloaderBuilder CreateParallelDownloader(this IAsyncEnumerable<IPhilomenaImage> images)
        {
            return new ParallelPhilomenaImageDownloaderBuilder(images);
        }
    }
}
