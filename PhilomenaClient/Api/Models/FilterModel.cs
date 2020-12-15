using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class FilterModel
    {
        /// <summary>
        /// The id of the filter.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the filter.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The description of the filter.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The id of the user the filter belongs to. null if it isn't assigned to a user (usually system filters only).
        /// </summary>
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        /// <summary>
        /// The amount of users employing this filter.
        /// </summary>
        [JsonPropertyName("user_count")]
        public int? UserCount { get; set; }

        /// <summary>
        /// If true, is a system filter. System filters are usable by anyone and don't have a user_id set.
        /// </summary>
        [JsonPropertyName("system")]
        public bool? IsSystem { get; set; }

        /// <summary>
        /// If true, is a public filter. Public filters are usable by anyone.
        /// </summary>
        [JsonPropertyName("public")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// A list of tag IDs (as ints) that this filter will spoil.
        /// </summary>
        [JsonPropertyName("spoilered_tag_ids")]
        public int[]? SpoileredTagIds { get; set; }

        /// <summary>
        /// The complex spoiled filter.
        /// </summary>
        [JsonPropertyName("spoilered_complex")]
        public string? SpoileredComplex { get; set; }

        /// <summary>
        /// A list of tag IDs (as ints) that this filter will hide.
        /// </summary>
        [JsonPropertyName("hidden_tag_ids")]
        public int[]? HiddenTagIds { get; set; }

        /// <summary>
        /// The complex hidden filter.
        /// </summary>
        [JsonPropertyName("hidden_complex")]
        public string? HiddenComplex { get; set; }
    }
}
