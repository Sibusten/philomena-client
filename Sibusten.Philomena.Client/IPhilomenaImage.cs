using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Utilities;

namespace Sibusten.Philomena.Client
{
    public interface IPhilomenaImage
    {
        /// <summary>
        /// The image model for this image
        /// </summary>
        ImageModel Model { get; }

        /// <summary>
        /// True if this is an SVG image (one which has both an SVG download and a raster download)
        /// </summary>
        bool IsSvgImage { get; }

        /// <summary>
        /// True if this represents the SVG version of an image, or false if it represents the raster version
        /// </summary>
        /// <value></value>
        bool IsSvgVersion { get; }

        int Id { get; }
        string? Name { get; }
        string? OriginalName { get; }
        string? Format { get; }
        string? Hash { get; }
        string? OriginalHash { get; }

        /// <summary>
        /// Downloads the image
        /// </summary>
        /// <returns>The image data</returns>
        Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null);

        /// <summary>
        /// Downloads the image to a stream
        /// </summary>
        /// <param name="stream">The stream to write the image data to</param>
        Task DownloadToAsync(Stream stream, CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null);

        /// <summary>
        /// Downloads the image data into a file
        /// </summary>
        /// <param name="file">The file to save the image to</param>
        Task DownloadToFileAsync(string file, CancellationToken cancellationToken = default, IProgress<StreamProgressInfo>? progress = null);
    }
}
