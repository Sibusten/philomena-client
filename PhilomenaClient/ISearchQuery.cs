using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sibusten.Philomena.Api;

namespace Sibusten.Philomena.Client
{
    /// <summary>
    /// A delegate that determines which file to save an image to
    /// </summary>
    /// <param name="image">The image being downloaded</param>
    /// <returns>The file to download the image to</returns>
    public delegate FileInfo GetFileForImageDelegate(IPhilomenaImage image);

    public interface ISearchQuery
    {
        /// <summary>
        /// Sets the filter for the query
        /// </summary>
        /// <param name="filterId">The ID of the filter to use</param>
        /// <returns>The search query</returns>
        ISearchQuery WithFilter(int filterId);

        /// <summary>
        /// Sets the sort order for the query
        /// </summary>
        /// <param name="sortField">The field to sort by</param>
        /// <param name="sortDirection">The direction to sort by</param>
        /// <returns>The search query</returns>
        ISearchQuery SortBy(SortField sortField, SortDirection sortDirection);

        /// <summary>
        /// Limits the number of images queried
        /// </summary>
        /// <param name="maxImages">The maximum number of images to query</param>
        /// <returns>The search query</returns>
        ISearchQuery Limit(int maxImages);

        /// <summary>
        /// Enumerates over the results of the query
        /// </summary>
        /// <returns>The search query</returns>
        IAsyncEnumerable<IPhilomenaImage> EnumerateResultsAsync();

        /// <summary>
        /// Gets the first image in the query
        /// </summary>
        /// <returns>The first image in the query</returns>
        Task<IPhilomenaImage> GetFirstAsync();

        /// <summary>
        /// Downloads all images in the query
        /// </summary>
        /// <param name="getFileForImage">A delegate that returns the file to download each image to</param>
        Task DownloadAllAsync(GetFileForImageDelegate getFileForImage);
    }
}
