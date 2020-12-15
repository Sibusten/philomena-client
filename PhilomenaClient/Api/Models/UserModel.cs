using System;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class UserModel
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The slug of the user.
        /// </summary>
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        /// <summary>
        /// The role of the user.
        /// </summary>
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        /// <summary>
        /// The description (bio) of the user.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The URL of the user's thumbnail. null if the avatar is not set.
        /// </summary>
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// The creation time, in UTC, of the user.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The comment count of the user.
        /// </summary>
        [JsonPropertyName("comments_count")]
        public int? CommentsCount { get; set; }

        /// <summary>
        /// The upload count of the user.
        /// </summary>
        [JsonPropertyName("uploads_count")]
        public int? UploadsCount { get; set; }

        /// <summary>
        /// The forum posts count of the user.
        /// </summary>
        [JsonPropertyName("posts_count")]
        public int? PostsCount { get; set; }

        /// <summary>
        /// The forum topics count of the user.
        /// </summary>
        [JsonPropertyName("topics_count")]
        public int? TopicsCount { get; set; }

        /// <summary>
        /// The links the user has registered. See <see cref="LinkModel"/>.
        /// </summary>
        [JsonPropertyName("links")]
        public LinkModel[]? Links { get; set; }

        /// <summary>
        /// The awards/badges of the user. See <see cref="AwardModel"/>.
        /// </summary>
        [JsonPropertyName("awards")]
        public AwardModel[]? Awards { get; set; }
    }
}
