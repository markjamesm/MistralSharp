using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MistralSharp.Dto;
using MistralSharp.Helpers;
using MistralSharp.Models;

namespace MistralSharp
{
    /// <summary>
    /// The MistralClient contains all the methods that can be used to interface with the Mistral AI platform.
    /// </summary>
    public class MistralClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://api.mistral.ai/v1";

        public MistralClient(string apiKey)
        {
            HttpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }
        
        
        /// <summary>
        /// Enables you to chat with an AI model on the Mistral platform.
        /// </summary>
        /// <param name="chatRequest">A ChatRequest object containing message and other data.</param>
        /// <returns>A ChatResponse object with the AI's response.</returns>
        public async Task<ChatResponse> ChatAsync(ChatRequest chatRequest)
        {
            var jsonResponse = await PostToApiAsync(chatRequest, "/chat/completions");
            var chatResponseDto = JsonSerializer.Deserialize<ChatResponseDto>(jsonResponse);

            var chatResponse = DtoMapper.MapChatResponse(chatResponseDto);
        
            return chatResponse;
        }
        
        
        /// <summary>
        /// Get the list of available models offered on the Mistral AI platform.
        /// </summary>
        /// <returns>An AvailableModels object</returns>
        public async Task<AvailableModels> GetAvailableModelsAsync()
        {
            var jsonResponse = await GetResponseAsync("/models");
            var availableModelsDto = JsonSerializer.Deserialize<AvailableModelsDto>(jsonResponse);

            var availableModels = DtoMapper.MapAvailableModels(availableModelsDto);
            
            return availableModels;
        }
        
        
        /// <summary>
        /// The embeddings API allows you to embed sentences and can be used to power a RAG application.
        /// </summary>
        /// <param name="embeddingRequest"></param>
        /// <returns>An EmbeddingResponse object.</returns>
        public async Task<EmbeddingResponse> CreateEmbeddingsAsync(EmbeddingRequest embeddingRequest)
        {
            var jsonResponse = await PostToApiAsync(embeddingRequest, "/embeddings");
            var embeddingResponseDto = JsonSerializer.Deserialize<EmbeddingResponseDto>(jsonResponse);
            
            var embeddingResponse = DtoMapper.MapEmbeddingResponse(embeddingResponseDto);
            
            return embeddingResponse;
        }
        
        
        /// <summary>
        /// Retrieves the response from the specified API endpoint asynchronously.
        /// </summary>
        /// <param name="endpoint">The API endpoint to send the request to.</param>
        /// <returns>The response content as a string.</returns>
        private static async Task<string> GetResponseAsync(string endpoint)
        {
            var returnMessage = await HttpClient.GetAsync(BaseUrl + (endpoint ?? "")).ConfigureAwait(false);

            return await returnMessage.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// Helper method to serialize an object to JSON and return a JSON response string.
        /// </summary>
        /// <param name="objectToSerialize">The object to be serialized.</param>
        /// <param name="endpoint">The endpoint to send the JSON request to.</param>
        /// <returns>A string representing the JSON response.</returns>
        private static async Task<string> PostToApiAsync(object objectToSerialize, string endpoint)
        {
            var jsonRequest = JsonSerializer.Serialize(objectToSerialize);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(BaseUrl + endpoint, content).ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }
    }
}