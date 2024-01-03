using Azure;
using Azure.AI.OpenAI;
using Azure.Core.Pipeline;
using Microsoft.SemanticKernel;
using OpenAiQuickstart.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSampleKernel();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();


app.MapGet("/weatherforecast", async (Kernel kernel) =>
{
    var temperatureC = Random.Shared.Next(-20, 55);
    var poemFunction = kernel.CreatePluginFromPromptDirectory("Prompts");
    var poem = await kernel.InvokeAsync<string>(poemFunction["poem"], new KernelArguments()
    {
        ["input"] = temperatureC
    });

    return new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now),
        temperatureC,
        poem
    );
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}