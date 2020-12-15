using System;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class TopicModel
    {
        /// <summary>
        /// The topic's slug (used to identify it).
        /// </summary>
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        /// <summary>
        /// The topic's title.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// The amount of posts in the topic.
        /// </summary>
        [JsonPropertyName("post_count")]
        public int? PostCount { get; set; }

        /// <summary>
        /// The amount of views the topic has received.
        /// </summary>
        [JsonPropertyName("view_count")]
        public int? ViewCount { get; set; }

        /// <summary>
        /// Whether the topic is sticky.
        /// </summary>
        [JsonPropertyName("sticky")]
        public bool? IsSticky { get; set; }

        /// <summary>
        /// The time, in UTC, when the last reply was made.
        /// </summary>
        [JsonPropertyName("last_replied_to_at")]
        public DateTime? LastRepliedToAt { get; set; }

        /// <summary>
        /// Whether the topic is locked.
        /// </summary>
        [JsonPropertyName("locked")]
        public bool? IsLocked { get; set; }

        /// <summary>
        /// The ID of the user who made the topic. null if posted anonymously.
        /// </summary>
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        /// <summary>
        /// The name of the user who made the topic.
        /// </summary>
        [JsonPropertyName("author")]
        public string? Author { get; set; }
    }
}
