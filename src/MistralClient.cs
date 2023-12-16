using System.Text;
using System.Text.Json;
using MistralSharp.Dto;
using MistralSharp.Models;
using Message = MistralSharp.Models.Message;
using ModelData = MistralSharp.Models.ModelData;
using Permission = MistralSharp.Models.Permission;
using Usage = MistralSharp.Models.Usage;

namespace MistralSharp;

public class MistralClient
{
    private static HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://api.mistral.ai/v1";
    private static string _apiKey;

    public MistralClient(string apiKey)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
    }
    
    private async Task<string> GetResponseAsync(string endpoint, string modelType = "")
    {
        var returnMessage = await _httpClient.GetAsync(_baseUrl + (endpoint ?? "")).ConfigureAwait(false);

        return await returnMessage.Content.ReadAsStringAsync();
    }

    public async Task<ChatResponse> ChatAsync(ChatRequest chatRequest)
    {
        var jsonRequest = JsonSerializer.Serialize(chatRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(_baseUrl + "/chat/completions", content).ConfigureAwait(false);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var chatResponseDto = JsonSerializer.Deserialize<ChatResponseDto>(jsonResponse);

        var chatResponse = new ChatResponse()
        {
            Id = chatResponseDto.Id,
            Created = chatResponseDto.Created,
            ObjectPropertyName = chatResponseDto.ObjectPropertyName,
            Model = chatResponseDto.Model,
            Usage = new Usage()
            {
                CompletionTokens = chatResponseDto.Usage.CompletionTokens,
                PromptTokens = chatResponseDto.Usage.PromptTokens,
                TotalTokens = chatResponseDto.Usage.TotalTokens
            },
            Choices = chatResponseDto.Choices.Select(c => new Choice()
            {
                Index = c.Index,
                FinishReason = c.FinishReason,
                Message = new ResponseMessage()
                {
                    Role = c.Message.Role,
                    Content = c.Message.Content
                }
            })
        };
        
        return chatResponse;
    }
    
    public async Task<AvailableModels> GetAvailableModelsAsync()
    {
        var jsonResponse = await GetResponseAsync("/models");
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