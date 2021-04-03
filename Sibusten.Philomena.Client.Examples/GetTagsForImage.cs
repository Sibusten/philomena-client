using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Images;

namespace Sibusten.Philomena.Client.Examples
{
    public class GetTagsForImage : IExample
    {
        public string Description => "Look up tags for an image";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");

            // Get an image
            IPhilomenaImage image = await client.GetImageSearch("fluttershy").BeginSearch().FirstAsync();

            Log.Information("Listing tags for image {ImageId}", image.Id);

            // Get the tags from the IDs
            foreach (int tagId in image.TagIds)
            {
                TagModel tag = await client.GetTagById(tagId);

                Log.Information("{TagId}: {TagName} (On {TagImages} images)", tagId, tag.Name, tag.Images);
            }
        }
    }
}
