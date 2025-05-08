using FloApi.Config;
using FloApi.Data;
using FloApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/floapi-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 🔹 Register services
builder.Services.AddControllers();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<TokenService>();

// 🛠️ Fix key to match your actual appsettings: "JwtSettings"
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// 🔹 Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔐 JWT Authentication setup
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var jwtKey = jwtSettings.SecretKey;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();   // ✅ This must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
