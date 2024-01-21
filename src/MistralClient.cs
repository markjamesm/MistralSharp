using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        private const string UserAgent = "MistralSharp";

        public MistralClient(string apiKey)
        {
            HttpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            
            HttpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        }
        
        
        /// <summary>
        /// Enables you to chat with an AI model on the Mistral platform. For streaming, use the StreamChatAsync()
        /// method instead.
        /// </summary>
        /// <param name="chatRequest">A ChatRequest object containing message and other data.</param>
        /// <returns>A ChatResponse object with the AI's response.</returns>
        public async Task<ChatResponse> ChatAsync(ChatRequest chatRequest)
        {
            if (chatRequest.Stream)
            {
                throw new InvalidOperationException("Error: stream parameter set to true. For streaming calls," +
                                                    "use the StreamChatAsync() method instead."); 
            }
            
            var jsonResponse = await PostToApiAsync(chatRequest, "/chat/completions");
            var chatResponseDto = JsonSerializer.Deserialize<ChatResponseDto>(jsonResponse);

            var chatResponse = DtoMapper.MapChatResponse(chatResponseDto);

            return chatResponse;
        }
        

        public async IAsyncEnumerable<ChatResponse> ChatStreamAsync(ChatRequest chatRequest)
        {
            if (chatRequest.Stream == false)
            {
                throw new InvalidOperationException("Error: ChatRequest.Stream is set to false. For " +
                                                    "non-streaming calls, use the ChatAsync() method instead."); 
            }
            
            var response = await HttpClient.GetAsync(BaseUrl + ("/chat/completions" ?? "")).ConfigureAwait(false);

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            var currentEvent = new SseEvent();
            
            while (await reader.ReadLineAsync() is { } line)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    currentEvent.Data = line.Substring("data:".Length).Trim();
                }
                else // an empty line indicates the end of an event
                {
                    if (currentEvent.Data == "[DONE]")
                    {
                        continue;
                    }
                    else if (currentEvent.EventType == null)
                    {
                        var res = await JsonSerializer.DeserializeAsync<ChatResponseDto>(
                            new MemoryStream(Encoding.UTF8.GetBytes(currentEvent.Data)));

                        var resMapped = DtoMapper.MapChatResponse(res);
                        
                        yield return resMapped;
                    }

                    // Reset the current event for the next one
                    currentEvent = new SseEvent();
                }
            }
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
            var response = await HttpClient.GetAsync(BaseUrl + (endpoint ?? "")).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return jsonResponse;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException(
                    "Mistral Platform had an internal server error. Please retry your request.");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Authorization error: Invalid API key.");
            }
            
            throw new HttpRequestException($"Unexpected HTTP status code: {response.StatusCode}");
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

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return jsonResponse;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException(
                    "Mistral Platform had an internal server error. Please retry your request.");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Authorization error: Invalid API key.");
            }
            
            throw new HttpRequestException($"Unexpected HTTP status code: {response.StatusCode}");
        }
    }
}