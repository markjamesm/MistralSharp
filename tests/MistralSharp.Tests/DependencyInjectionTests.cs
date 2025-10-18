using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MistralSharp.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void DependencyInjection_ShouldWork()
    {
        var builder = new ConfigurationBuilder();
        var configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => configuration);

        serviceCollection.AddMistral(options =>
        {
            options.ApiKey = "test-api-key";
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var client = serviceProvider.GetRequiredService<IMistralClient>();
    }
}