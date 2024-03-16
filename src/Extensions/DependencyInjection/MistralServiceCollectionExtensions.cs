using Microsoft.Extensions.DependencyInjection;
using MistralSharp.Abstractions;
using MistralSharp.Domain;
using System;

namespace MistralSharp.Extensions.DependencyInjection
{
    public static class MistralServiceCollectionExtensions
    {
        public static IServiceCollection AddMistral(this IServiceCollection services, Action<MistralClientOptions> setupAction)
        {
            var optionsBuilder = services.AddOptions<MistralClientOptions>();

            if (setupAction != null)
            {
                optionsBuilder.Configure(setupAction);
            }
            else
            {
                optionsBuilder.BindConfiguration(MistralClientOptions.SettingKey);
            }

            return services.AddScoped<IMistralClient, MistralClient>();
        }
    }
}
