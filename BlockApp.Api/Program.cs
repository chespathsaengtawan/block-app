
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BlockApp.Api.Data;
using BlockApp.Api.Middlewares;
using BlockApp.Api.Services;
using BlockApp.Api.Services.Sms;
using BlockApp.Api.Services.Interfaces;
using BlockApp.Api.Services.Interfaces.Sms;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

#endregion

#region Services

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    }
);

// OpenAPI (Built-in .NET 10)
builder.Services.AddOpenApi();

// Database - SQLite (Dev) / PostgreSQL (Production)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsProduction())
    {
        // Railway provides DATABASE_URL (postgres://user:pass@host:port/db)
        // Fallback to ConnectionStrings:DefaultConnection env var
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        string connStr;
        if (!string.IsNullOrEmpty(databaseUrl))
        {
            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':', 2);
            connStr = $"Host={uri.Host};Port={uri.Port};" +
                      $"Database={uri.AbsolutePath.TrimStart('/')};" +
                      $"Username={userInfo[0]};Password={Uri.UnescapeDataString(userInfo[1])};" +
                      $"SSL Mode=Require;Trust Server Certificate=true";
        }
        else
        {
            connStr = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException(
                          "No PostgreSQL connection string found. Set DATABASE_URL or ConnectionStrings__DefaultConnection.");
        }
        options.UseNpgsql(connStr);
    }
    else
    {
        options.UseSqlite(
            builder.Configuration.GetConnectionString("DefaultConnection")
        );
    }
});

// ===== Authentication (JWT) =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),

            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// ===== Application Services =====
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<RateLimitService>();
builder.Services.AddScoped<IOtpService, OtpService>();

// ===== Points & Payment Services =====
builder.Services.AddScoped<IOmiseService, OmiseService>();
builder.Services.AddScoped<IPointsService, PointsService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// ===== SMS Services =====
builder.Services.AddScoped<ITwilioSmsService, TwilioSmsService>();
builder.Services.AddScoped<IThaibulkSmsService, ThaibulkSmsService>();
builder.Services.AddHttpClient("ThaibulkSMS", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

#endregion

#region Build App

var app = builder.Build();

#endregion

#region Database Initialization

// Dev (SQLite): apply migrations | Prod (PostgreSQL): EnsureCreated (schema from model)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (app.Environment.IsProduction())
        db.Database.EnsureCreated();
    else
        db.Database.Migrate();
}

#endregion

#region Middleware Pipeline

// Swagger (เฉพาะ Dev)
//if OpenAPI Documentation (Dev only)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// ✅ Global Exception Middleware (ต้องอยู่บนสุด)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Standard ASP.NET middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check for Railway
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

#endregion

#region Run

// Railway injects PORT env var; fallback to 8080
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");

#endregion
