# MistralSharp

[![C#](https://img.shields.io/badge/Language-CSharp-darkgreen.svg)](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) [![NuGet](https://img.shields.io/nuget/vpre/MistralSharp)](https://www.nuget.org/packages/MistralSharp) [![License](https://img.shields.io/badge/License-MIT-red.svg)](https://opensource.org/licenses/MIT)

## About

MistralSharp is an unofficial .NET SDK for the [Mistral AI Platform](https://mistral.ai/). Great for building
AI-enhanced apps!

## Features

* Implements all Mistral AI Platform REST API endpoints.
* Targets .NET Standard 2.0 and .NET 8.

## Usage

Start by [downloading the nuget package](https://www.nuget.org/packages/MistralSharp) and importing it into your project.

Check out the [Sample project](https://github.com/markjamesm/MistralSharp/blob/main/sample/Program.cs) to see an example of how to use the library in a simple console application.

To access the API endpoints, create a new instance of the `MistralClient` class and pass in your API key:

```csharp
var mistralClient = new MistralClient(apiKey);
```

## Endpoints

### GetAvailableModelsAsync()
This endpoint returns a list of available AI models on the Mistral platform.

```csharp
var models = await mistralClient.GetAvailableModelsAsync();
```

### ChatAsync()

This method allows you to chat with an AI model of your choice. To start a chat, first create a new `ChatRequest` 
object (note: only Model and Messages are required, the other parameters are optional and will default to the values 
specified below):

```csharp
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
    MaxTokens = 64,
    
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
```

Finally, call the `ChatAsync()` method and pass the `ChatRequest` object:
```csharp
var sampleChat = await mistralClient.ChatAsync(chatRequest);
```

### CreateEmbeddingsAsync()

The embeddings API allows you to embed sentences and can be used to power a RAG application. To use it, first 
create a create a new `EmbeddingRequest` object:

```csharp
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
```

Next, pass the `EmbeddingRequest` object to `ChatEmbeddingsAsync()` method:

```csharp
var embeddedResponse = await mistralClient.CreateEmbeddingsAsync(embeddings);
```