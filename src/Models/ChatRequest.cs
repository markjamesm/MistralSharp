using System.Text.Json.Serialization;

namespace MistralSharp.Models;

public class ChatRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.7;
    
    [JsonPropertyName("top_p")]
    public int TopP { get; set; } = 1;
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; } = null;
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
    
    [JsonPropertyName("safe_mode")]
    public bool SafeMode { get; set; } = false;
    
    [JsonPropertyName("random_seed")]
    public int? RandomSeed { get; set; } = null;
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
}