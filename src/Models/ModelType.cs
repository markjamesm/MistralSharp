namespace MistralSharp.Models;

public sealed class ModelType
{
    // Embedding models enable retrieval and retrieval-augmented generation applications.
    public static string MistralEmbed => "mistral-embed";
    
    // This generative endpoint is best used for large batch processing tasks where cost is a significant factor but reasoning capabilities are not crucial.
    public static string MistralTiny => "mistral-tiny";
    
    // Higher reasoning capabilities and more capabilities.
    // The endpoint supports English, French, German, Italian, and Spanish and can produce and reason about code.
    public static string MistralSmall => "mistral-small";
    
    // This endpoint currently relies on an internal prototype model.
    public static string MistralMedium => "mistral-medium";
}