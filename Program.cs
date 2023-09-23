using NewsManagementMinimal.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<NewsDataContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/AllNews", async () => await new NewsDataContext(builder.Configuration).GetData())
.WithName("GetAllNews")
.WithOpenApi();




app.Run();