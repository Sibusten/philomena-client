using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Sibusten.Philomena.Client.Extensions;
using Sibusten.Philomena.Client.Images;
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

            SyncProgress<ImageSearchProgressInfo> searchProgress = new SyncProgress<ImageSearchProgressInfo>(ProgressReport);

            // Run the download on another thread
            Task downloadTask = Task.Run(async () =>
            {
                try
                {
                    await client
                        .SearchImages("fluttershy")
                            .WithMaxImages(100)
                        .BeginSearch(cts.Token, searchProgress: searchProgress)
                        .CreateParallelDownloader()
                            .WithImageFileDownloader(image => Path.Join("ExampleDownloads", "PauseAndCancelImageDownload", $"{image.Id}.{image.Format}"))
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

        private void ProgressReport(ImageSearchProgressInfo downloadProgress)
        {
            Log.Information("{ImagesProcessed}/{ImagesTotal}", downloadProgress.Processed, downloadProgress.Total);
        }
    }
}
