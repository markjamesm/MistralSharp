using MistralSharp.Abstractions;
using MistralSharp.Domain;
using MistralSharp.Dto;
using MistralSharp.Helpers;
using MistralSharp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MistralSharp
{
    /// <inheritdoc />
    public class MistralClient : IMistralClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://api.mistral.ai/v1";

        public MistralClient(MistralClientOptions options)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IAsyncEnumerable<ChatResponse> ChatStreamAsync(ChatRequest chatRequest)
        {
            if (chatRequest.Stream == false)
            {
                throw new InvalidOperationException("Error: ChatRequest.Stream is set to false. For " +
                                                    "non-streaming calls, use the ChatAsync() method instead.");
            }

            throw new NotImplementedException("Error: This endpoint is not yet implemented but will be in " +
                                              "a future release.");
        }

        /// <inheritdoc />
        public async Task<AvailableModels> GetAvailableModelsAsync()
        {
            var jsonResponse = await GetResponseAsync("/models");
            var availableModelsDto = JsonSerializer.Deserialize<AvailableModelsDto>(jsonResponse);

            var availableModels = DtoMapper.MapAvailableModels(availableModelsDto);

            return availableModels;
        }

        /// <inheritdoc />
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