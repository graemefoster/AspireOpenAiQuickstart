using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetTopologySuite.Geometries;
using OpenAiQuickstart.BusinessApi;
using OpenAiQuickstart.BusinessDomain;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("ApiOptions"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Banking API",
        Description = "An ASP.NET Core Banking API to demonstrate interesting Open AI use-cases",
    });
    
    options.MapType<Point>(() => new OpenApiSchema()
    {
        Description = "Latitude and Longitude coordinates",
        Type = "array",
        Example = new OpenApiArray()
        {
            new OpenApiDouble(-31.78),
            new OpenApiDouble(115.76),
        },
        MinItems = 2,
        MaxItems = 2
    });
    
    var xmlFilename = $"{typeof(BankingContext).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    xmlFilename = $"{typeof(Program).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new MoneyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new PointJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters
        .Add(new MoneyJsonConverter());
    options.SerializerOptions.Converters
        .Add(new PointJsonConverter());
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<BankingContext>(
    (sp, options) =>
        options.UseSqlServer(sp.GetRequiredService<IOptions<Configuration>>().Value.ConnectionString,
            sqlOptions => sqlOptions.UseNetTopologySuite()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/account/{accountNumber}",
        (BankingContext context, Guid accountNumber) => context.Accounts.FindAsync(accountNumber).Result)
    .WithName("GetAccount")
    .WithOpenApi();

app.MapGet("/account/{accountNumber}/transactions",
        (BankingContext context, Guid accountNumber) => context.AccountTransactions.Where(x => x.From == accountNumber)
            .OrderByDescending(x => x.Date))
    .WithName("GetAccountTransactions")
    .WithOpenApi();

app.MapControllers();

app.Run();