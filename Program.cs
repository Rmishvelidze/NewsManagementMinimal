
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsManagementMinimal.Data;
using NewsManagementMinimal.Models;
using NewsManagementMinimal.Repositories.News;
using NewsManagementMinimal.Repositories.User;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.MapPost("/login",
        (UserLogin user, IUserRepository userRepo) => Login(user, userRepo))
    .Accepts<UserLogin>("application/json")
    .Produces<string>();

app.MapGet("/AllNews",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        async (INewsRepository newsRepository) => await newsRepository.GetAllNews())
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


IResult Login(UserLogin user, IUserRepository userRepo)
{
    if (!string.IsNullOrEmpty(user.Username) &&
        !string.IsNullOrEmpty(user.Password))
    {
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
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(tokenString);
    }
    return Results.BadRequest("Invalid user credentials");
}

app.Run();