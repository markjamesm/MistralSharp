using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MistralSharp.Dto;
using MistralSharp.Models;
using ModelData = MistralSharp.Models.ModelData;
using Permission = MistralSharp.Models.Permission;

namespace MistralSharp
{
    /// <summary>
    /// The MistralClient contains all the methods that can be used to interface with the Mistral AI platform.
    /// </summary>
    public class MistralClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _baseUrl = "https://api.mistral.ai/v1";

        public MistralClient(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }
    
        private static async Task<string> GetResponseAsync(string endpoint, string modelType = "")
        {
            var returnMessage = await _httpClient.GetAsync(_baseUrl + (endpoint ?? "")).ConfigureAwait(false);

            return await returnMessage.Content.ReadAsStringAsync();
        }

        
        /// <summary>
        /// Enables you to chat with an AI model on the Mistral platform.
        /// </summary>
        /// <param name="chatRequest"></param>
        /// <returns>A ChatResponse object</returns>
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
        
        /// <summary>
        /// Get the list of available models offered on the Mistral AI platform.
        /// </summary>
        /// <returns>An AvailableModels object</returns>
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
        
        public async Task<EmbeddingResponse> CreateEmbeddingsAsync(EmbeddingRequest embeddingRequest)
        {
            var jsonRequest = JsonSerializer.Serialize(embeddingRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + "/embeddings", content).ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var embeddingResponseDto = JsonSerializer.Deserialize<EmbeddingResponseDto>(jsonResponse);
            var embeddingResponse = new EmbeddingResponse()
            {
                Id = embeddingResponseDto.Id,
                Model = embeddingResponseDto.Model,
                Object = embeddingResponseDto.Object,
                Data = embeddingResponseDto.Data.Select(d => new Embedding()
                {
                    Index = d.Index,
                    Object = d.Object,
                    EmbeddingList = d.Embedding
                }).ToList(),
                TokenUsage = new TokenUsage()
                {
                    PromptTokens = embeddingResponseDto.Usage.PromptTokens,
                    TotalTokens = embeddingResponseDto.Usage.TotalTokens
                }
                
            };
            return embeddingResponse;
        }
    }
}