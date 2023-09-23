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




app.Run();