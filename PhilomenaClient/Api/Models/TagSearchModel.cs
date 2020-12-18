using System.Collections.Generic;
using Newtonsoft.Json;

namespace Philomena.Client.Api.Models
{
    public class TagSearchModel
    {
        [JsonProperty("tags")]
        public List<TagModel>? Tags { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }
    }
}
