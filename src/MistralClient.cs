using System.Text.Json;
using MistralSharp.Dto;
using MistralSharp.Models;

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
            Object = availableModelsDeserialized?.Object
        };

        foreach (var model in availableModelsDeserialized?.Data)
        {
            availableModels.AvailableModelsList.Add(new AvailableModelsData()
            {
                Created = model.Created,
                Id = model.Id,
                Object = model.Object,
                OwnedBy = model.OwnedBy
            });
        }

        return availableModels;
    }
}