using System;
using System.Linq;
using System.Threading.Tasks;
using Philomena.Client.Api;
using Philomena.Client.Api.Models;

namespace Philomena.Client.Examples.Api
{
    public class SearchTags : IExample
    {
        public string Description => "Search for tags";

        public async Task RunExample()
        {
            Console.WriteLine("Creating API client");
            PhilomenaApi api = new PhilomenaApi("https://derpibooru.org");

            string searchQuery = "id:247962";
            Console.WriteLine("Using search to get a tag by ID");
            Console.WriteLine($"Using search query: '{searchQuery}'");

            TagSearchModel tagSearch = await api.SearchTagsAsync(searchQuery);

            TagModel tag = tagSearch.Tags.First();

            Console.WriteLine($"Tag name: {tag.Name}");
            Console.WriteLine($"Tag description: {tag.Description}");

            DnpEntryModel dnpEntry = tag.DnpEntries.First();
            Console.WriteLine($"DNP Reason: {dnpEntry.Reason}");
        }
    }
}
