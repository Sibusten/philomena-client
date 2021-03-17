using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Sibusten.Philomena.Client.Logging;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Images.Downloaders
{
    public abstract class PhilomenaImageDownloader : IPhilomenaDownloader<IPhilomenaImage>
    {
        private ILogger _logger;

        public PhilomenaImageDownloader()
        {
            _logger = Logger.Factory.CreateLogger(GetType());
        }

        public abstract Task Download(IPhilomenaImage downloadItem, CancellationToken cancellationToken = default, IProgress<DownloadProgressInfo>? progress = null);

        protected async Task<Stream> GetDownloadStream(IPhilomenaImage image, CancellationToken cancellationToken, IProgress<StreamProgressInfo>? progress)
        {
            IFlurlResponse response = await image.ShortViewUrl.GetAsync(cancellationToken, HttpCompletionOption.ResponseHeadersRead);

            // Attempt to read the length of the stream from the header
            long? length = null;
            if (response.Headers.TryGetFirst("Content-Length", out string lengthString))
            {
                if (long.TryParse(lengthString, out long parsedLength))
                {
                    length = parsedLength;
                }
            }

            // Open the image stream
            Stream downloadStream = await response.GetStreamAsync();

            _logger.LogDebug("Opened download stream for image {ImageId} with size {DownloadSize}: {DownloadUrl}", image.Id, length, image.ShortViewUrl);

            // Create progress stream wrapper for reporting download progress
            return new StreamProgressReporter(downloadStream, progress, length);
        }
    }
}
