namespace MistralSharp.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class EmbeddingResponseDto 
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("data")]
        public List<EmbeddingObject> Data { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("usage")]
        public EmbedUsage Usage { get; set; }
    }

    public class EmbeddingObject
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }
    
        [JsonPropertyName("embedding")]
        public List<float> Embedding { get; set; }
    
        [JsonPropertyName("index")]
        public int Index { get; set; }
    }

    public class EmbedUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }
    
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}