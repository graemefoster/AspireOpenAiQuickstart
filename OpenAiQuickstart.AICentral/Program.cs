using AICentral.Configuration;
using AICentral.Core;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults(
    meters => { meters.AddMeter(AICentralActivitySource.AICentralTelemetryName); },
    tracers => { tracers.AddSource(AICentralActivitySource.AICentralTelemetryName); }
);

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddAICentral(builder.Configuration);
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();

app.UseAICentral();

app.Run();