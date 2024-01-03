using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAiQuickstart.BusinessApi;
using OpenAiQuickstart.BusinessDomain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("ApiOptions"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankingContext>(
    (sp, options) =>
        options.UseSqlServer(sp.GetRequiredService<IOptions<Configuration>>().Value.ConnectionString,
            sqlOptions => sqlOptions.MigrationsAssembly("OpenAiQuickstart.BusinessApi.DbBuilder")));
;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/account/{accountNumber}",
        (BankingContext context, Guid accountNumber) => context.Accounts.Find(accountNumber))
    .WithName("GetAccount")
    .WithOpenApi();

app.Run();