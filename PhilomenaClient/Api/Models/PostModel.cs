using System;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class PostModel
    {
        /// <summary>
        /// The post's author.
        /// </summary>
        [JsonPropertyName("author")]
        public string? Author { get; set; }

        /// <summary>
        /// The URL of the author's avatar. May be a link to the CDN path, or a data: URI.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// The post text.
        /// </summary>
        [JsonPropertyName("body")]
        public string? Body { get; set; }

        /// <summary>
        /// The creation time, in UTC, of the post.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The edit reason for this post.
        /// </summary>
        [JsonPropertyName("edit_reason")]
        public string? EditReason { get; set; }

        /// <summary>
        /// The time, in UTC, this post was last edited at, or null if it was not edited.
        /// </summary>
        [JsonPropertyName("edited_at")]
        public DateTime? EditedAt { get; set; }

        /// <summary>
        /// The post's ID (used to identify it).
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The time, in UTC, the post was last updated at.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The ID of the user the post belongs to, if any.
        /// </summary>
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
    }
}
