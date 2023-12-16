using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MistralSharp.Dto
{
    // DTO to get the available models from the Mistral API
    public class AvailableModelsDto
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }
    
        [JsonPropertyName("data")]
        public List<ModelData> Data { get; set; }
    }

    public class ModelData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    
        [JsonPropertyName("object")]
        public string Object { get; set; }
    
        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("owned_by")]
        public string OwnedBy { get; set; }
    
        [JsonPropertyName("root")]
        public object Root { get; set; }
    
        [JsonPropertyName("parent")]
        public object Parent { get; set; }
    
        [JsonPropertyName("permission")]
        public List<Permission> Permission { get; set; }
    }

    public class Permission
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    
        [JsonPropertyName("object")]
        public string Object { get; set; }
    
        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("allow_create_engine")]
        public bool AllowCreateEngine { get; set; }

        [JsonPropertyName("allow_sampling")]
        public bool AllowSampling { get; set; }

        [JsonPropertyName("allow_logprobs")]
        public bool AllowLogprobs { get; set; }
    
        [JsonPropertyName("allow_search_indices")]
        public bool AllowSearchIndices { get; set; }
    
        [JsonPropertyName("allow_view")]
        public bool AllowView { get; set; }
    
        [JsonPropertyName("allow_fine_tuning")]
        public bool AllowFineTuning { get; set; }
    
        [JsonPropertyName("organization")]
        public string Organization { get; set; }
    
        [JsonPropertyName("group")]
        public object Group { get; set; }
    
        [JsonPropertyName("is_blocking")]
        public bool IsBlocking { get; set; }
    }
}