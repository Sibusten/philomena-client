using Flurl;
using Flurl.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;
using System.Threading;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaImage : IPhilomenaImage
    {
        public ImageModel Model { get; init; }

        public PhilomenaImage(ImageModel model)
        {
            Model = model;
        }

        public int Id
        {
            get
            {
                if (Model.Id is null)
                {
                    throw new InvalidOperationException("Image is missing an ID");
                }

                return Model.Id.Value;
            }
        }

        public string Name
        {
            get
            {
                if (Model.ViewUrl is null)
                {
                    return "";
                }

                string localPath = new Uri(Model.ViewUrl).LocalPath;
                return Path.GetFileNameWithoutExtension(localPath);
            }
        }

        public string OriginalName
        {
            get
            {
                if (Model.Name is null)
                {
                    return "";
                }

                Path.GetFileNameWithoutExtension(Model.Name);
            }
        }

        public Url DownloadUrl
        {
            get
            {
                if (Model.Representations is null)
                {
                    throw new InvalidOperationException("The image has no representations");
                }

                if (Model.Representations.Full is null)
                {
                    throw new InvalidOperationException("The image is missing a 'full' representation");
                }

                return new Url(Model.Representations.Full);
            }
        }

        public async Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await DownloadToAsync(memoryStream, cancellationToken, progress);

            // Extract the image data from the memory stream
            return memoryStream.ToArray();
        }

        public async Task DownloadToAsync(Stream stream, CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null)
        {
            using Stream downloadStream = await DownloadUrl.GetStreamAsync(cancellationToken);

            // Create progress stream wrapper for reporting download progress
            using StreamProgressReporter progressStream = new StreamProgressReporter(downloadStream, progress);

            await progressStream.CopyToAsync(stream, cancellationToken);
        }

        public async Task DownloadToFileAsync(string file, CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null)
        {
            string? imageDirectory = Path.GetDirectoryName(file);
            if (imageDirectory is null)
            {
                throw new ArgumentException("The file does not have a parent directory", nameof(file));
            }
            Directory.CreateDirectory(imageDirectory);

            using FileStream fileStream = File.OpenWrite(file);
            await DownloadToAsync(fileStream, cancellationToken, progress);
        }
    }
}
