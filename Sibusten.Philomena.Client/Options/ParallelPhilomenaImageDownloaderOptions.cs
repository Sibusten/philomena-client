using System.Collections.Generic;
using Sibusten.Philomena.Client.Images;

namespace Sibusten.Philomena.Client.Options
{
    public record ParallelPhilomenaImageDownloaderOptions
    {
        /// <summary>
        /// The maximum threads to use when downloading images. Defaults to 1.
        /// </summary>
        public int MaxDownloadThreads { get; init; } = 1;

        /// <summary>
        /// The downloaders to use for each image
        /// </summary>
        public ICollection<IPhilomenaDownloader<IPhilomenaImage>> Downloaders = new List<IPhilomenaDownloader<IPhilomenaImage>>();
    }
}
