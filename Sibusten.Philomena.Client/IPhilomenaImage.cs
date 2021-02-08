using System;
using System.Collections.Generic;
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
        List<string> TagNames { get; }
        List<int> TagIds { get; }
        int? Score { get; }
        int? FileSize { get; }
        string? SourceUrl { get; }
        bool? IsSpoilered { get; }
        int? TagCount { get; }
        bool? ThumbnailsGenerated { get; }
        DateTime? UpdatedAt { get; }
        string? Uploader { get; }
        int? UploaderId { get; }
        int? Upvotes { get; }
        string? ViewUrl { get; }
        bool? Processed { get; }
        string? MimeType { get; }
        bool? IsAnimated { get; }
        double? AspectRatio { get; }
        int? CommentCount { get; }
        DateTime? CreatedAt { get; }
        string? DeletionReason { get; }
        string? Description { get; }
        int? Downvotes { get; }
        int? Width { get; }
        int? DuplicateOf { get; }
        int? Faves { get; }
        DateTime? FirstSeenAt { get; }
        int? Height { get; }
        bool? IsHiddenFromUsers { get; }
        double? Duration { get; }
        double? WilsonScore { get; }

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
