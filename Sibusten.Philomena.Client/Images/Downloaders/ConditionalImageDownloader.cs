using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Images.Downloaders
{
    public delegate bool ShouldDownloadImageDelegate(IPhilomenaImage image);

    public class ConditionalImageDownloader : PhilomenaImageDownloader
    {
        private readonly IPhilomenaDownloader<IPhilomenaImage> _downloader;
        private readonly ShouldDownloadImageDelegate _shouldDownloadImage;

        public ConditionalImageDownloader(ShouldDownloadImageDelegate shouldDownloadImage, IPhilomenaDownloader<IPhilomenaImage> downloader)
        {
            _shouldDownloadImage = shouldDownloadImage;
            _downloader = downloader;
        }

        public override async Task Download(IPhilomenaImage downloadItem, CancellationToken cancellationToken = default, IProgress<DownloadProgressInfo>? progress = null)
        {
            if (_shouldDownloadImage(downloadItem))
            {
                await _downloader.Download(downloadItem, cancellationToken, progress);
            }
        }
    }
}
