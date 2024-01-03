var builder = DistributedApplication.CreateBuilder(args);

var aiCentral = builder.AddProject<Projects.OpenAiQuickstart_AICentral>("aicentral");
var apiservice = builder.AddProject<Projects.OpenAiQuickstart_ApiService>("apiservice")
    .WithReference(aiCentral);

builder.AddProject<Projects.OpenAiQuickstart_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
