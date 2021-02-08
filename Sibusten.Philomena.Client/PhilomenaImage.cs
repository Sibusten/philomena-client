using Flurl;
using Flurl.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;
using System.Threading;
using Sibusten.Philomena.Client.Utilities;
using System.Net.Http;

namespace Sibusten.Philomena.Client
{
    public class PhilomenaImage : IPhilomenaImage
    {
        public ImageModel Model { get; private init; }
        private readonly int _id;
        public bool IsSvgVersion { get; init; } = false;

        public bool IsSvgImage => Model.Format == "svg";

        public PhilomenaImage(ImageModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id is null)
            {
                throw new ArgumentNullException("Image is missing an ID", nameof(model.Id));
            }

            Model = model;
            _id = model.Id.Value;
        }

        public int Id => _id;

        public string? Name
        {
            get
            {
                if (Model.ViewUrl is null)
                {
                    return null;
                }

                string localPath = new Uri(Model.ViewUrl).LocalPath;
                return Path.GetFileNameWithoutExtension(localPath);
            }
        }

        public string? OriginalName
        {
            get
            {
                if (Model.Name is null)
                {
                    return null;
                }

                return Path.GetFileNameWithoutExtension(Model.Name);
            }
        }

        public Url? DownloadUrl
        {
            get
            {
                if (Model.Representations is null || Model.Representations.Full is null)
                {
                    return null;
                }

                if (IsSvgVersion)
                {
                    // Modify the full URL to point to the SVG image
                    string urlWithoutExtension = Model.Representations.Full.Substring(0, Model.Representations.Full.LastIndexOf('.'));
                    return new Url(urlWithoutExtension + ".svg");
                }

                return new Url(Model.Representations.Full);
            }
        }

        public string? Format
        {
            get
            {
                if (IsSvgImage)
                {
                    // The image is an SVG image, which has two possible formats
                    // Assume rasters are always png
                    return IsSvgVersion ? "svg" : "png";
                }

                return Model.Format;
            }
        }

        public string? Hash => Model.Sha512Hash;
        public string? OriginalHash => Model.OrigSha512Hash;

        public async Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await DownloadToAsync(memoryStream, cancellationToken, progress);

            // Extract the image data from the memory stream
            return memoryStream.ToArray();
        }

        public async Task DownloadToAsync(Stream stream, CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null)
        {
            using IFlurlResponse response = await DownloadUrl.GetAsync(cancellationToken, HttpCompletionOption.ResponseHeadersRead);

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
            using Stream downloadStream = await response.GetStreamAsync();

            // Create progress stream wrapper for reporting download progress
            using StreamProgressReporter progressStream = new StreamProgressReporter(downloadStream, progress, length);

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
