using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class GalleryModel
    {
        /// <summary>
        /// The gallery's description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The gallery's ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The gallery's spoiler warning.
        /// </summary>
        [JsonPropertyName("spoiler_warning")]
        public string? SpoilerWarning { get; set; }

        /// <summary>
        /// The ID of the cover image for the gallery.
        /// </summary>
        [JsonPropertyName("thumbnail_id")]
        public int? ThumbnailId { get; set; }

        /// <summary>
        /// The gallery's title.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// The name of the gallery's creator.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

        /// <summary>
        /// The ID of the gallery's creator.
        /// </summary>
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
    }
}
