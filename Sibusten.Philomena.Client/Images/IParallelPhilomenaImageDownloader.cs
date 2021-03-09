using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Images
{
    public interface IParallelPhilomenaImageDownloader
    {
        /// <summary>
        /// Begins a parallel download
        /// </summary>
        /// <param name="imagesToDownload">The images to download</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="progress">A list of progress reporters. There must be one progress reporter given for each download slot</param>
        Task BeginDownload(IAsyncEnumerable<IPhilomenaImage> imagesToDownload, CancellationToken cancellationToken = default, IReadOnlyCollection<IProgress<DownloadProgressInfo>>? progress = null);
    }
}
