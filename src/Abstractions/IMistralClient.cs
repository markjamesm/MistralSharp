using MistralSharp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralSharp.Abstractions
{
    /// <summary>
    /// The MistralClient contains all the methods that can be used to interface with the Mistral AI platform.
    /// </summary>
    public interface IMistralClient
    {
        /// <summary>
        /// Enables you to chat with an AI model on the Mistral platform. For streaming, use the StreamChatAsync()
        /// method instead.
        /// </summary>
        /// <param name="chatRequest">A ChatRequest object containing message and other data.</param>
        /// <returns>A ChatResponse object with the AI's response.</returns>
        Task<ChatResponse> ChatAsync(ChatRequest chatRequest);

        IAsyncEnumerable<ChatResponse> ChatStreamAsync(ChatRequest chatRequest);

        /// <summary>
        /// Get the list of available models offered on the Mistral AI platform.
        /// </summary>
        /// <returns>An AvailableModels object</returns>
        Task<AvailableModels> GetAvailableModelsAsync();

        /// <summary>
        /// The embeddings API allows you to embed sentences and can be used to power a RAG application.
        /// </summary>
        /// <param name="embeddingRequest"></param>
        /// <returns>An EmbeddingResponse object.</returns>
        Task<EmbeddingResponse> CreateEmbeddingsAsync(EmbeddingRequest embeddingRequest);
    }
}
