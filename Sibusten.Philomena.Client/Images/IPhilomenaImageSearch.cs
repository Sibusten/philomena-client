using System;
using System.Collections.Generic;
using System.Threading;

namespace Sibusten.Philomena.Client.Images
{
    public struct MetadataDownloadProgressInfo
    {
        public int Downloaded { get; set; }
        public int Total { get; set; }
    }

    public interface IPhilomenaImageSearch
    {
        IAsyncEnumerable<IPhilomenaImage> BeginSearch(CancellationToken cancellationToken = default, IProgress<MetadataDownloadProgressInfo>? progress = null);
    }
}
