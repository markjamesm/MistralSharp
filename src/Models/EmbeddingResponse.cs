using System.Collections.Generic;

namespace MistralSharp.Models
{
    public class EmbeddingResponse 
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public IEnumerable<Embedding> Data { get; set; }
        public string Model { get; set; }
        public TokenUsage TokenUsage { get; set; }
    }

    public class Embedding
    {
        public string Object { get; set; }
        public IEnumerable<float> EmbeddingList { get; set; }
        public int Index { get; set; }
    }

    public class TokenUsage
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}