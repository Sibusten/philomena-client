using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sibusten.Philomena.Api;

namespace Sibusten.Philomena.Client
{
    /// <summary>
    /// A delegate that determines which file to save an image to
    /// </summary>
    /// <param name="image">The image being downloaded</param>
    /// <returns>The file to download the image to</returns>
    public delegate string GetFileForImageDelegate(IPhilomenaImage image);

    /// <summary>
    /// A delegate that determines which stream to save an image to
    /// </summary>
    /// <param name="image">The image being downloaded</param>
    /// <returns>The stream to download the image to</returns>
    public delegate Stream GetStreamForImageDelegate(IPhilomenaImage image);

    /// <summary>
    /// A delegate that determines whether an image should be downloaded or skipped. Skipped images still count toward limits.
    /// </summary>
    /// <param name="image">The image being considered</param>
    /// <returns>True if the image should be downloaded</returns>
    public delegate bool ShouldDownloadImageDelegate(IPhilomenaImage image);

    public struct MetadataDownloadProgressInfo
    {
        public int Downloaded { get; set; }
        public int Total { get; set; }
    }

    public struct ImageDownloadProgressInfo
    {
        public int ImagesDownloaded { get; set; }
        public int MetadataDownloaded { get; set; }
        public int TotalImages { get; set; }
        public ParallelImageDownloadProgressInfo[] Downloads { get; init; }
    }

    public struct ParallelImageDownloadProgressInfo
    {
        public int ImageId { get; set; }
        public long BytesDownloaded { get; set; }
        public long? BytesTotal { get; set; }
    }

    public interface IPhilomenaImageSearchQuery
    {
        /// <summary>
        /// Sets the filter for the query
        /// </summary>
        /// <param name="filterId">The ID of the filter to use</param>
        /// <returns>The search query</returns>
        IPhilomenaImageSearchQuery WithFilter(int filterId);

        /// <summary>
        /// Sets the sort order for the query
        /// </summary>
        /// <param name="sortField">The field to sort by</param>
        /// <param name="sortDirection">The direction to sort by</param>
        /// <returns>The search query</returns>
        IPhilomenaImageSearchQuery SortBy(SortField sortField, SortDirection sortDirection);

        /// <summary>
        /// Limits the number of images queried
        /// </summary>
        /// <param name="maxImages">The maximum number of images to query</param>
        /// <returns>The search query</returns>
        IPhilomenaImageSearchQuery Limit(int maxImages);

        /// <summary>
        /// Sets the maximum threads to use when downloading images
        /// </summary>
        /// <param name="maxDownloadThreads">The maximum threads to use when downloading images</param>
        /// <returns>The search query</returns>
        IPhilomenaImageSearchQuery WithMaxDownloadThreads(int maxDownloadThreads);

        /// <summary>
        /// Enumerates over the results of the query
        /// </summary>
        /// <returns>The search query</returns>
        IAsyncEnumerable<IPhilomenaImage> EnumerateResultsAsync(CancellationToken cancellationToken = default, IProgress<MetadataDownloadProgressInfo>? progress = null);

        /// <summary>
        /// Gets the first image in the query
        /// </summary>
        /// <returns>The first image in the query</returns>
        Task<IPhilomenaImage> GetFirstAsync();

        /// <summary>
        /// Downloads all images in the query
        /// </summary>
        /// <param name="getStreamForImage">A delegate that returns the stream to download each image to</param>
        /// <param name="leaveOpen">Whether the stream should be left open after writing</param>
        Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen = false, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null);

        /// <summary>
        /// Downloads all images in the query that pass a custom filter
        /// </summary>
        /// <param name="getStreamForImage">A delegate that returns the stream to download each image to. The stream will be disposed after downloading.</param>
        /// <param name="shouldDownloadImage">A delegate that determines whether an image should be downloaded</param>
        /// <remarks>Note: many conditions can be specified directly and provide better performance if done that way. Only use the custom filter if the condition cannot be added otherwise.</remarks>
        Task DownloadAllAsync(GetStreamForImageDelegate getStreamForImage, bool leaveOpen, ShouldDownloadImageDelegate shouldDownloadImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null);

        /// <summary>
        /// Downloads all images in the query to files
        /// </summary>
        /// <param name="getFileForImage">A delegate that returns the file to download each image to</param>
        Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null);

        /// <summary>
        /// Downloads all images in the query that pass a custom filter to files
        /// </summary>
        /// <param name="getFileForImage">A delegate that returns the file to download each image to</param>
        /// <param name="shouldDownloadImage">A delegate that determines whether an image should be downloaded</param>
        /// <remarks>Note: many conditions can be specified directly and provide better performance if done that way. Only use the custom filter if the condition cannot be added otherwise.</remarks>
        Task DownloadAllToFilesAsync(GetFileForImageDelegate getFileForImage, ShouldDownloadImageDelegate shouldDownloadImage, CancellationToken cancellationToken = default, IProgress<ImageDownloadProgressInfo>? progress = null);
    }
}
