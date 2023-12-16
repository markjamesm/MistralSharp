using Microsoft.Extensions.Configuration;
using MistralSharp;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddUserSecrets<Program>();
var configuration = configurationBuilder.Build();

// API key is stored in dotnet secrets
var apiKey = configuration["MistralAPIKey"];

// Create a new instance of MistralClient
var mistralClient = new MistralClient();

// Get the models endpoint response and print the result to console
var models = await mistralClient.GetAvailableModelsAsync(apiKey);

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

// var modelsList = models.Result.AvailableModelsList;

/*
foreach (var mistralModels in modelsList)
{
    Console.WriteLine($"Model ID: {mistralModels.Id}");
    Console.WriteLine($"Model Object: {mistralModels.Object}");
    Console.WriteLine($"Model Created: {mistralModels.Created}");
    Console.WriteLine($"Model OwnedBy: {mistralModels.OwnedBy}");
}
*/
