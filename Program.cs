using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsManagementMinimal.Data;
using NewsManagementMinimal.Middleware;
using NewsManagementMinimal.Models;
using NewsManagementMinimal.Repositories.News;
using NewsManagementMinimal.Repositories.User;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
        lc.ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.File(
                path: builder.Configuration["LogFilePath"] + "/" + DateTime.Today.ToString("yy.MM.dd") + "/",
                rollingInterval: RollingInterval.Hour))
    .ConfigureAppConfiguration((_, configurationBuilder) =>
    {
        configurationBuilder.AddEnvironmentVariables();
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<UserDataContext>();
builder.Services.AddSingleton<NewsDataContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware(typeof(CustomMiddleware));

app.UseHttpsRedirection();

#region Endpionts
app.MapPost("/login",
        (UserLogin user, IUserRepository userRepo) => Login(user, userRepo))
    .Accepts<UserLogin>("application/json")
    .Produces<string>();

app.MapGet("/AllNews",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
    async (INewsRepository newsRepository) => await newsRepository.GetAllNews())
    .WithName("GetAllNews")
    .Produces<string>();

app.MapGet("/NewsByDays",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
async (int days, INewsRepository newsRepository) => await newsRepository.GetNewsByDays(days)!)
    .WithName("GetNewsByDays")
    .Produces<string>();

app.MapGet("/NewsByText",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
    async (string text, INewsRepository newsRepository) => await newsRepository.GetNewsByText(text))
    .WithName("GetNewsByText")
    .Produces<string>();

app.MapGet("/Latest5News", async (INewsRepository newsRepository) => await newsRepository.GetLatest5News()!)
    .WithName("GetLatest5News")
    .Produces<string>();

app.MapPost("/Subscribe",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        (INewsRepository newsRepository) => newsRepository.Subscribe())
    .WithName("Subscribe")
    .Produces<string>();
#endregion

app.Run();
return;

IResult Login(UserLogin user, IUserRepository userRepo)
{
    if (string.IsNullOrEmpty(user.Username) ||
        string.IsNullOrEmpty(user.Password)) return Results.BadRequest("Invalid user credentials");
    var loggedInUser = userRepo.Get(user);
    if (loggedInUser is null) return Results.NotFound("User not found");

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
        new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
        new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
        new Claim(ClaimTypes.Surname, loggedInUser.Surname),
        new Claim(ClaimTypes.Role, loggedInUser.Role)
    };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
            SecurityAlgorithms.HmacSha256)
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(tokenString);
}