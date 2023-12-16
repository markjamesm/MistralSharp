namespace MistralSharp.Models;

public class ChatResponse
{
    public string Id { get; set; }
    public string ObjectPropertyName { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public IEnumerable<Choice> Choices { get; set; }
    public Usage Usage { get; set; }
}

public class Choice
{
    public int Index { get; set; }
    public ResponseMessage Message { get; set; }
    public string FinishReason { get; set; }
}

public class ResponseMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}

public class Usage
{
    public int PromptTokens { get; set; }
    public int TotalTokens { get; set; }
    public int CompletionTokens { get; set; }
}