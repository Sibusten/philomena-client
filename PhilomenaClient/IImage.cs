using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Client
{
    public interface IImage
    {
        /// <summary>
        /// The image model for this image
        /// </summary>
        ImageModel Model { get; }

        /// <summary>
        /// Downloads the image
        /// </summary>
        /// <returns>The image data</returns>
        Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the image to a stream
        /// </summary>
        /// <param name="stream">The stream to write the image data to</param>
        Task DownloadToAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the image data into a file
        /// </summary>
        /// <param name="file">The file to save the image to</param>
        Task DownloadToFileAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}
