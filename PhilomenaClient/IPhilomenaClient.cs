using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Client
{
    public interface IPhilomenaClient
    {
        /// <summary>
        /// Begins a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns></returns>
        ISearchQuery Search(string query);

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
