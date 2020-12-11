using System.Collections.Generic;

namespace Philomena.Client
{
    public enum SortDirection
    {
        Descending,
        Ascending
    }

    public enum SortField
    {
        ImageId,
    }

    public interface ISearchQuery
    {
        /// <summary>
        /// Sets the filter for the query
        /// </summary>
        /// <param name="filterId">The ID of the filter to use</param>
        /// <returns></returns>
        ISearchQuery WithFilter(int filterId);

        /// <summary>
        /// Sets the sort order for the query
        /// </summary>
        /// <param name="sortField">The field to sort by</param>
        /// <param name="sortDirection">The direction to sort by</param>
        /// <returns></returns>
        ISearchQuery SortBy(SortField sortField, SortDirection sortDirection);

        /// <summary>
        /// Enumerates over the results of the query
        /// </summary>
        /// <returns></returns>
        IAsyncEnumerable<IImage> EnumerateResultsAsync();
    }
}
