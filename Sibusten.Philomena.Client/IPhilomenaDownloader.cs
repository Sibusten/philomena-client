using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sibusten.Philomena.Client
{
    public interface IPhilomenaDownloader<TDownload>
    {
        Task Download(TDownload downloadItem, CancellationToken cancellationToken = default, IProgress<DownloadProgressInfo>? progress = null);
    }
}
