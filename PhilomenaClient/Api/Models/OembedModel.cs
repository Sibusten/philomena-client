using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class OembedModel
    {
        /// <summary>
        /// The comma-delimited names of the image authors.
        /// </summary>
        [JsonPropertyName("author_name")]
        public string? AuthorName { get; set; }

        /// <summary>
        /// The source URL of the image.
        /// </summary>
        [JsonPropertyName("author_url")]
        public string? AuthorUrl { get; set; }

        /// <summary>
        /// Always 7200.
        /// </summary>
        [JsonPropertyName("cache_age")]
        public int? CacheAge { get; set; }

        /// <summary>
        /// The number of comments made on the image.
        /// </summary>
        [JsonPropertyName("derpibooru_comments")]
        public int? DerpibooruComments { get; set; }

        /// <summary>
        /// The image's ID.
        /// </summary>
        [JsonPropertyName("derpibooru_id")]
        public int? DerpibooruId { get; set; }

        /// <summary>
        /// The image's number of upvotes minus the image's number of downvotes.
        /// </summary>
        [JsonPropertyName("derpibooru_score")]
        public int? DerpibooruScore { get; set; }

        /// <summary>
        /// The names of the image's tags.
        /// </summary>
        [JsonPropertyName("derpibooru_tags")]
        public string[]? DerpibooruTags { get; set; }

        /// <summary>
        /// Always "Derpibooru".
        /// </summary>
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; set; }

        /// <summary>
        /// Always "https://derpibooru.org".
        /// </summary>
        [JsonPropertyName("provider_url")]
        public string? ProviderUrl { get; set; }

        /// <summary>
        /// The image's ID and associated tags, as would be given on the title of the image page.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Always "photo".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Always "1.0".
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }
}
