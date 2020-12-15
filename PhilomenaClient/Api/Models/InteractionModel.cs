using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class InteractionModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("interaction_type")]
        public string? InteractionType { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        [JsonPropertyName("image_id")]
        public int? ImageId { get; set; }
    }
}
