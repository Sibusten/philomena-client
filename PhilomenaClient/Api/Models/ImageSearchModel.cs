using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class ImageSearchModel
    {
        [JsonPropertyName("images")]
        public List<ImageModel>? Images { get; set; }

        [JsonPropertyName("interactions")]
        public List<InteractionModel>? Interactions { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
