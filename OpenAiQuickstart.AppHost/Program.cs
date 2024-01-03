var builder = DistributedApplication.CreateBuilder(args);

var aiCentral = builder.AddProject<Projects.OpenAiQuickstart_AICentral>("aicentral");

var bankingapi = builder.AddProject<Projects.OpenAiQuickstart_BusinessApi>("bankingapi");

var apiservice = builder.AddProject<Projects.OpenAiQuickstart_ApiService>("apiservice")
    .WithReference(aiCentral)
    .WithReference(bankingapi);

builder.AddProject<Projects.OpenAiQuickstart_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
