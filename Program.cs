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

app.MapGet("/NewsByDays", (int days,INewsRepository newsRepository) => newsRepository.GetNewsByDays(days))
    .WithName("GetNewsByDays")
    .WithOpenApi();

app.MapGet("/NewsByText", (string text, INewsRepository newsRepository) => newsRepository.GetNewsByText(text))
    .WithName("GetNewsByText")
    .WithOpenApi();

app.Run();