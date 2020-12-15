using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class ImageSearchModel
    {
        [JsonPropertyName("images")]
        public ImageModel[]? Images { get; set; }

        [JsonPropertyName("interactions")]
        public InteractionModel[]? Interactions { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
