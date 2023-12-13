namespace MistralSharp.Models;

public class AvailableModels
{
    public string? Object { get; set; }
    public List<AvailableModelsData>? AvailableModelsList { get; set; }
}

public class AvailableModelsData
{
    public string? Id { get; set; }
    public string? Object { get; set; }
    public int Created { get; set; }
    public string? OwnedBy { get; set; }
}