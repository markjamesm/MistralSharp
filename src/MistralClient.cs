using System.Text.Json;
using MistralSharp.Dto;
using MistralSharp.Models;
using ModelData = MistralSharp.Models.ModelData;
using Permission = MistralSharp.Models.Permission;

namespace MistralSharp;

public class MistralClient
{
    private static HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://api.mistral.ai/v1";
    
    private async Task<string> GetResponseAsync(string endpoint, string apiKey, string modelType = "")
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        
        var returnMessage = await _httpClient.GetAsync(_baseUrl + (endpoint ?? "")).ConfigureAwait(false);

        return await returnMessage.Content.ReadAsStringAsync();
    }

    
    public async Task<AvailableModels> GetAvailableModelsAsync(string apiKey)
    {
        var jsonResponse = await GetResponseAsync("/models", apiKey);
        var availableModelsDeserialized = JsonSerializer.Deserialize<AvailableModelsDto>(jsonResponse);

        var availableModels = new AvailableModels()
        {
            Object = availableModelsDeserialized?.Object,
            Data = availableModelsDeserialized.Data.Select(d => new ModelData()
            {
                Created = d.Created,
                Id = d.Id,
                Object = d.Object,
                OwnedBy = d.OwnedBy,
                Parent = d.Parent,
                Root = d.Root,
                Permission = d.Permission.Select(p => new Permission()
                {
                    Id = p.Id,
                    Object = p.Object,
                    Created = p.Created,
                    Organization = p.Organization,
                    Group = p.Group,
                    IsBlocking = p.IsBlocking,
                    AllowCreateEngine = p.AllowCreateEngine,
                    AllowFineTuning = p.AllowFineTuning,
                    AllowLogprobs = p.AllowLogprobs,
                    AllowSampling = p.AllowSampling,
                    AllowSearchIndices = p.AllowSearchIndices,
                    AllowView = p.AllowView
                }).ToList()
            }).ToList()
            
        };
            
        return availableModels;
    }
}