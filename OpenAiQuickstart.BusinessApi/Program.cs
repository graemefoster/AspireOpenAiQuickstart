using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAiQuickstart.BusinessApi;
using OpenAiQuickstart.BusinessDomain;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("ApiOptions"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters
        .Add(new MoneyJsonConverter());
    options.SerializerOptions.Converters
        .Add(new PointJsonConverter());
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

app.Run();