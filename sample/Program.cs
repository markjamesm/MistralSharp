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


// Get the models endpoint response and print the result to console
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

// Create a new chat
var chatRequest = new ChatRequest()
{
    
    // The ID of the model to use. You can use GetAvailableModelsAsync() to get the list of available models
    Model = "mistral-medium",
    
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
    SafeMode = false,
    
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