using System;
using System.Collections.Generic;
using System.Threading;

namespace Sibusten.Philomena.Client.Images
{
    public record MetadataDownloadProgressInfo
    {
        public int Downloaded { get; init; }
        public int Total { get; init; }
    }

    public record ImageSearchProgressInfo
    {
        public int Processed { get; init; }
        public int Total { get; init; }
    }

    public interface IPhilomenaImageSearch
    {
        IAsyncEnumerable<IPhilomenaImage> BeginSearch(CancellationToken cancellationToken = default, IProgress<ImageSearchProgressInfo>? searchProgress = null, IProgress<MetadataDownloadProgressInfo>? metadataProgress = null);
    }
}
