using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class TagSearchModel
    {
        [JsonPropertyName("tags")]
        public List<TagModel>? Tags { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
