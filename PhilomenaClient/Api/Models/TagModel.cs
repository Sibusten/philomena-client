using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class TagModel
    {
        /// <summary>
        /// The slug of the tag this tag is aliased to, if any.
        /// </summary>
        [JsonPropertyName("aliased_tag")]
        public string? AliasedTag { get; set; }

        /// <summary>
        /// The slugs of the tags aliased to this tag.
        /// </summary>
        [JsonPropertyName("aliases")]
        public string[]? Aliases { get; set; }

        /// <summary>
        /// The category class of this tag. One of "character", "content-fanmade", "content-official", "error", "oc", "origin", "rating", "species", "spoiler".
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>
        /// The long description for the tag.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// An array of objects containing DNP entries claimed on the tag.
        /// </summary>
        [JsonPropertyName("dnp_entries")]
        public int[]? DnpEntries { get; set; }

        /// <summary>
        /// The tag's ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// The image count of the tag.
        /// </summary>
        [JsonPropertyName("images")]
        public int? Images { get; set; }

        /// <summary>
        /// The slugs of the tags this tag is implied by.
        /// </summary>
        [JsonPropertyName("implied_by_tags")]
        public string[]? ImpliedByTags { get; set; }

        /// <summary>
        /// The slugs of the tags this tag implies.
        /// </summary>
        [JsonPropertyName("implied_tags")]
        public string[]? ImpliedTags { get; set; }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The name of the tag in its namespace.
        /// </summary>
        [JsonPropertyName("name_in_namespace")]
        public string? NameInNamespace { get; set; }

        /// <summary>
        /// The namespace of the tag.
        /// </summary>
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        /// <summary>
        /// The short description for the tag.
        /// </summary>
        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        /// <summary>
        /// The slug for the tag.
        /// </summary>
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        /// <summary>
        /// The spoiler image for the tag.
        /// </summary>
        [JsonPropertyName("spoiler_image_uri")]
        public string? SpoilerImageUri { get; set; }
    }

    public class TagResponseModel
    {
        [JsonPropertyName("tag")]
        public TagModel? Tag { get; set; }
    }
}
