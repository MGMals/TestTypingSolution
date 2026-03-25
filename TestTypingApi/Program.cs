using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TestTypingApi.Models.Context;
using TestTypingApi.Service;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<TestTypingSpeedsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<TokenService>();

// Read JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = !string.IsNullOrEmpty(issuer),
        ValidateAudience = !string.IsNullOrEmpty(audience),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Define CORS = Cross-Origin Resource Sharing
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();
// browser security feature that controls which websites are allowed to call your API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Must be placed before UseAuthorization and MapControllers
app.UseCors("BlazorClient");



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();