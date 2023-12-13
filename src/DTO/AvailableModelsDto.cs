namespace MistralSharp.Dto;

// DTO to get the available models from the Mistral API
public class AvailableModelsDataDto
{
    public string? Id { get; set; }
    public string? Object { get; set; }
    public int Created { get; set; }
    public string? OwnedBy { get; set; }
}

public class AvailableModelsDto
{
    public string? Object { get; set; }
    public List<AvailableModelsDataDto>? Data { get; set; }
}