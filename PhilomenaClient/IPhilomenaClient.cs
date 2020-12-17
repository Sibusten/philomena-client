namespace Philomena.Client
{
    public interface IPhilomenaClient
    {
        /// <summary>
        /// Begins a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns></returns>
        ISearchQuery Search(string query);
    }
}
