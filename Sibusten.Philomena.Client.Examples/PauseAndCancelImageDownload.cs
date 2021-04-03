using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images.Downloaders;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client.Examples
{
    public class PauseAndCancelImageDownload : IExample
    {
        public string Description => "Pause and cancel a download in progress";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");

            using CancellationTokenSource cts = new CancellationTokenSource();

            SyncProgress<PhilomenaImageSearchDownloadProgressInfo> searchProgress = new SyncProgress<PhilomenaImageSearchDownloadProgressInfo>(ProgressReport);

            // Run the download on another thread
            Task downloadTask = Task.Run(async () =>
            {
                try
                {
                    await client
                        .GetImageSearch("fluttershy", o => o
                            .WithMaxImages(100)
                        )
                        .CreateParallelDownloader(maxDownloadThreads: 1, o => o
                            .WithImageFileDownloader(image => Path.Join("ExampleDownloads", "PauseAndCancelImageDownload", $"{image.Id}.{image.Format}"))
                        )
                        .BeginDownload(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Log.Information("Download cancelled");
                }
            });

            // Wait a bit before canceling
            Log.Information("Downloading some images");
            await Task.Delay(3000);

            // Cancel the download
            Log.Information("Cancelling the download");
            cts.Cancel();

            // Wait for the download thread to finish
            await downloadTask;

            Log.Information("Download ended");
        }

        private void ProgressReport(PhilomenaImageSearchDownloadProgressInfo downloadProgress)
        {
            Log.Information("{ImagesDownloaded}/{ImagesTotal}", downloadProgress.ImagesDownloaded, downloadProgress.ImagesTotal);
        }
    }
}
