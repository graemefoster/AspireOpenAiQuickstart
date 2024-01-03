using Azure;
using Azure.AI.OpenAI;
using Azure.Core.Pipeline;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace OpenAiQuickstart.ApiService;

public static class SemanticKernel
{
    public static IServiceCollection AddSampleKernel(this IServiceCollection services)
    {
        services.AddTransient<KernelPluginCollection>();

        services.AddSingleton<Kernel>(sp =>
        {
            var oaiClient = new OpenAIClient(new Uri("http://aicentral"), new AzureKeyCredential("sdfsdfsdf1"),
                new OpenAIClientOptions()
                {
                    Transport = new HttpClientTransport(sp.GetService<IHttpClientFactory>()!.CreateClient())
                });

            var kernelBuilder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion("Gpt35Turbo0613", oaiClient);

            kernelBuilder.Plugins.AddFromPromptDirectory("Prompts");

            return kernelBuilder.Build();
        });

        return services;
    }
}