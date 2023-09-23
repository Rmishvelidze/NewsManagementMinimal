using Microsoft.Extensions.Caching.Memory;
using NewsManagementMinimal.Data;
using NewsManagementMinimal.Repositories.News;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<NewsDataContext>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/AllNews", async (INewsRepository newsRepository) => await newsRepository.GetAllNews())
    .WithName("GetAllNews")
    .WithOpenApi();

app.MapGet("/NewsByDays", async (int days,INewsRepository newsRepository) => await newsRepository.GetNewsByDays(days)!)
    .WithName("GetNewsByDays")
    .WithOpenApi();

app.MapGet("/NewsByText", async (string text, INewsRepository newsRepository) => await newsRepository.GetNewsByText(text))
    .WithName("GetNewsByText")
    .WithOpenApi();

app.MapGet("/Latest5News", async (INewsRepository newsRepository) => await newsRepository.GetLatest5News()!)
    .WithName("GetLatest5News")
    .WithOpenApi();

app.MapPost("/Subscribe", (INewsRepository newsRepository) => newsRepository.Subscribe())
    .WithName("Subscribe")
    .WithOpenApi();

app.Run();