using System;
using System.Threading.Tasks;
using Sibusten.Philomena.Api.Models;

namespace Sibusten.Philomena.Client.Examples
{
    public class GetTagsForImage : IExample
    {
        public string Description => "Look up tags for an image";

        public async Task RunExample()
        {
            PhilomenaClient client = new PhilomenaClient("https://derpibooru.org");

            // Get an image
            IPhilomenaImage image = await client.Search("fluttershy").GetFirstAsync();

            Console.WriteLine($"Tags for image {image.Id}:");

            // Get the tags from the IDs
            foreach (int tagId in image.TagIds)
            {
                TagModel tag = await client.GetTagById(tagId);

                Console.WriteLine($"{tagId}: {tag.Name} (On {tag.Images} images)");
            }
        }
    }
}
