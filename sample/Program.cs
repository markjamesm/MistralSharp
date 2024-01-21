using Microsoft.Extensions.Configuration;
using MistralSharp;
using MistralSharp.Models;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddUserSecrets<Program>();
var configuration = configurationBuilder.Build();

// API key is stored in dotnet secrets
var apiKey = configuration["MistralAPIKey"];

// Create a new instance of MistralClient and pass your API key
var mistralClient = new MistralClient(apiKey);


// ----------------------------------------
// Get the models endpoint response and print the result to console
// ----------------------------------------
var models = await mistralClient.GetAvailableModelsAsync();

Console.WriteLine($"ID: {models.Object}");

foreach (var modelData in models.Data)
{
    Console.WriteLine($"\nModel ID: {modelData.Id}\n " +
                      $"Model Object: {modelData.Object}\n" +
                      $"Model Created: {modelData.Created}\n" +
                      $"Model OwnedBy: {modelData.OwnedBy}\n" +
                      $"Model Root: {modelData.Root}\n" +
                      $"Model Parent: {modelData.Parent}"
    );
    
    Console.WriteLine("Permissions:");

    foreach (var permission in modelData.Permission)
    {
        Console.WriteLine($"ID: {permission.Id}\n" +
                          $"Object: {permission.Object}\n" +
                          $"Created: {permission.Created}\n" +
                          $"Organization: {permission.Organization}\n" +
                          $"Group: {permission.Group}\n" +
                          $"IsBlocking: {permission.IsBlocking}\n" +
                          $"AllowCreateEngine: {permission.AllowCreateEngine}\n" +
                          $"AllowFineTuning: {permission.AllowFineTuning}\n" +
                          $"AllowLogprobs: {permission.AllowLogprobs}\n" +
                          $"AllowSampling: {permission.AllowSampling}\n" +
                          $"AllowSearchIndices: {permission.AllowSearchIndices}\n" +
                          $"AllowView: {permission.AllowView}"
        );
    }
}

Console.WriteLine("-------------------------");

// ----------------------------------------
// Make a non-streamining call to the chat API
// ----------------------------------------

// Create a new ChatRequest object
var chatRequest = new ChatRequest()
{
    
    // The ID of the model to use. You can use GetAvailableModelsAsync() to get the list of available models
    Model = ModelType.MistralMedium,
    
    // Pass a list of messages to the model. 
    // The role can either be "user" or "agent"
    // Content is the message content
    Messages =
    [
        new Message()
        {
            Role = "user",
            Content = "How can Mistral AI assist programmers?"
        }
    ],
    
    //The maximum number of tokens to generate in the completion.
    // The token count of your prompt plus max_tokens cannot exceed the model's context length.
    MaxTokens = 16,
    
    //  Default: 0.7
    // What sampling temperature to use, between 0.0 and 2.0.
    // Higher values like 0.8 will make the output more random, while lower values like 0.2 will make
    // it more focused and deterministic.
    Temperature = 0.7,
    
    //  Default: 1
    // Nucleus sampling, where the model considers the results of the tokens with top_p probability mass.
    // So 0.1 means only the tokens comprising the top 10% probability mass are considered.
    // Mistral generally recommends altering this or temperature but not both.
    TopP = 1,
    
    //  Default: false
    // Whether to stream back partial progress. If set, tokens will be sent as data-only server-sent events
    // as they become available, with the stream terminated by a data: [DONE] message. Otherwise, the server will
    // hold the request open until the timeout or until completion, with the response containing the full
    // result as JSON.
    Stream = false,
    
    //  Default: false
    // Whether to inject a safety prompt before all conversations.
    SafePrompt = false,
    
    //  Default: null
    // The seed to use for random sampling. If set, different calls will generate deterministic results.
    RandomSeed = null
};

// Call the chat endpoint and pass our ChatRequest object
var sampleChat = await mistralClient.ChatAsync(chatRequest);

Console.WriteLine($"\nChat Response ID: {sampleChat.Id}\n" + 
                  $"Chat Response Created: {sampleChat.Created}\n" + 
                  $"Chat Response Object: {sampleChat.ObjectPropertyName}\n" + 
                  $"Chat Response Model: {sampleChat.Model}\n" + 
                  $"Chat Response Usage:\n" + 
                  $"Prompt Tokens: {sampleChat.Usage.PromptTokens}\n" + 
                  $"Completion Tokens: {sampleChat.Usage.CompletionTokens}\n" + 
                  $"Total Tokens: {sampleChat.Usage.TotalTokens}\n");

Console.WriteLine("Choices:");
foreach (var choice in sampleChat.Choices)
{
    Console.WriteLine($"Finish Reason: {choice.FinishReason}\n" + $"Index: {choice.Index}\n");
    Console.WriteLine("Response Messages:");
    Console.WriteLine($"Role: {choice.Message.Role}\n" + $"Content: {choice.Message.Content}\n");
}

Console.WriteLine("-------------------------");


// ----------------------------------------
// Make a streaming call to the chat API
// ----------------------------------------

// Create a new chat
// For a full explanation of each property, see the chatRequest object above
var chatStreamRequest = new ChatRequest()
{
    Model = ModelType.MistralMedium,
    Messages =
    [
        new Message()
        {
            Role = "user",
            Content = "What is an LLM?"
        }
    ],
    MaxTokens = 60,
    Temperature = 0.7,
    TopP = 1,
    Stream = true,
    SafePrompt = false,
    RandomSeed = null
};

// Call the ChatStreamAsync endpoint and pass our ChatRequest object
var sampleChatStream = mistralClient.ChatStreamAsync(chatStreamRequest);

await foreach (var response in sampleChatStream)
{
    Console.WriteLine($"\nChat Response ID: {response.Id}\n" + 
                      $"Chat Response Created: {response.Created}\n" + 
                      $"Chat Response Object: {response.ObjectPropertyName}\n" + 
                      $"Chat Response Model: {response.Model}\n" + 
                      $"Chat Response Usage:\n" + 
                      $"Prompt Tokens: {response.Usage.PromptTokens}\n" + 
                      $"Completion Tokens: {response.Usage.CompletionTokens}\n" + 
                      $"Total Tokens: {response.Usage.TotalTokens}\n");
    
    foreach (var item in response.Choices)
    {
        Console.WriteLine($"Finish Reason: {item.FinishReason}\n" + $"Index: {item.Index}\n");
        Console.WriteLine("Response Messages:");
        Console.WriteLine($"Role: {item.Message.Role}\n" + $"Content: {item.Message.Content}\n");
    }
}

Console.WriteLine("-------------------------");


// ----------------------------------------
// Embed an object using the embeddings API.
// ----------------------------------------

// Create a new EmbeddingRequest object
var embeddings = new EmbeddingRequest()
{
    // The ID of the model to use for this request.
    Model = ModelType.MistralEmbed,
    
    // The format of the output data.
    EncodingFormat = "float",
    
    // The list of strings to embed.
    Input = new List<string>()
    {
        "Hello",
        "World"
    }
};

// Create an embedding and pass it our EmbeddingRequest object
var embeddedResponse = await mistralClient.CreateEmbeddingsAsync(embeddings);

// Print the embedding to the console
Console.WriteLine("\n---Example Embedding Response---");
Console.WriteLine("EmbeddingResponse:\nId: {0}\nObject: {1}\nModel: {2}\nPromptTokens: {3}\nTotalTokens: {4}",
    embeddedResponse.Id,
    embeddedResponse.Object,
    embeddedResponse.Model,
    embeddedResponse.TokenUsage?.PromptTokens,
    embeddedResponse.TokenUsage?.TotalTokens);
        
foreach (var embedding in embeddedResponse.Data)
{
    Console.WriteLine("  - Object: {0}\n    Index: {1}\n    EmbeddingList: {2}",
        embedding.Object,
        embedding.Index,
        string.Join(", ", embedding.EmbeddingList));
}

Console.WriteLine("-------------------------");