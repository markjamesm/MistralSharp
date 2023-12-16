using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MistralSharp.Dto;

public class ChatResponseDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("object")]
    public string ObjectPropertyName { get; set; }
    
    [JsonPropertyName("created")]
    public int Created { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("choices")]
    public IEnumerable<ChoiceDto> Choices { get; set; }
    
    [JsonPropertyName("usage")]
    public UsageDto Usage { get; set; }
}

public class ChoiceDto
{
    [JsonPropertyName("index")]
    public int Index { get; set; }
    
    [JsonPropertyName("message")]
    public MessageDto Message { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

public class MessageDto
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class UsageDto
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
    
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
}