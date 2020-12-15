using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class ForumModel
    {
        /// <summary>
        /// The forum's name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The forum's short name (used to identify it).
        /// </summary>
        [JsonPropertyName("short_name")]
        public string? ShortName { get; set; }

        /// <summary>
        /// The forum's description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The amount of topics in the forum.
        /// </summary>
        [JsonPropertyName("topic_count")]
        public int? TopicCount { get; set; }

        /// <summary>
        /// The amount of posts in the forum.
        /// </summary>
        [JsonPropertyName("post_count")]
        public int? PostCount { get; set; }
    }
}
