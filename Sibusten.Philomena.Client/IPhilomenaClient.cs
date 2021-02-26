using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Images;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client
{
    public interface IPhilomenaClient
    {
        /// <summary>
        /// Begins a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="options">Options for the search query</param>
        /// <returns>An image search</returns>
        IPhilomenaImageSearch Search(string query, ImageSearchOptions? options = null);

        /// <summary>
        /// Gets a tag by its ID
        /// </summary>
        /// <param name="tagId">The ID of the tag</param>
        /// <returns>The tag model</returns>
        Task<TagModel> GetTagById(int tagId);

        /// <summary>
        /// Gets a tag by its name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <returns>The tag model</returns>
        // TagModel GetTagByName(string tagName)
    }
}
