using System.Linq;
using MistralSharp.Dto;
using MistralSharp.Models;
using ModelData = MistralSharp.Models.ModelData;
using Permission = MistralSharp.Models.Permission;

namespace MistralSharp.Helpers
{
    public static class DtoMapper
    {
        internal static AvailableModels MapAvailableModels(AvailableModelsDto availableModelsDto)
        {
            var availableModels = new AvailableModels()
            {
                Object = availableModelsDto.Object,
                Data = availableModelsDto.Data.Select(d => new ModelData()
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

        internal static ChatResponse MapChatResponse(ChatResponseDto chatResponseDto)
        {
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
        
        internal static EmbeddingResponse MapEmbeddingResponse(EmbeddingResponseDto embeddingResponseDto)
        {
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