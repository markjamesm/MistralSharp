using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MistralSharp.Models
{
    public class EmbeddingRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("input")]
        public IEnumerable<string> Input { get; set; }

        [JsonPropertyName("encoding_format")]
        public string EncodingFormat { get; set; }
    }
}