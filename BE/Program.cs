using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using BE.Services.UserService;
using BE.Services.TokenService;
using BE.Services.EmailService;
using BE.Services.CategoryService;
using BE.Repositories.UserRepo;
using BE.Repositories.CategoryRepo;
using BE.Repositories.ProductRepo;
using BE.Services.ProductService;
using BE.Repositories.OrderRepo;
using BE.Services.OrderService;
using PayOS;

LoadEnvFile();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    // Đọc danh sách domain được phép từ config (appsettings / .env)
    // Dev: http://localhost:5254 | Production: https://vuon.vn
    var allowedOrigins = builder.Configuration.GetSection("App:AllowedOrigins").Get<string[]>()
        ?? new[] { "http://localhost:5254" };

    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// Đăng ký Repositories & Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton(sp => {
    var cfg = sp.GetRequiredService<IConfiguration>();
    return new PayOSClient(
        GetRequiredConfiguration(cfg, "PayOS:ClientId"),
        GetRequiredConfiguration(cfg, "PayOS:ApiKey"),
        GetRequiredConfiguration(cfg, "PayOS:ChecksumKey")
    );
});


var googleCookieSecurePolicy = builder.Environment.IsDevelopment()
    ? CookieSecurePolicy.SameAsRequest
    : CookieSecurePolicy.Always;

var googleCookieSameSite = builder.Environment.IsDevelopment()
    ? SameSiteMode.Lax
    : SameSiteMode.None;



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
//thêm cookie để lưu thông tin trước khi controller xử lý
.AddCookie("External", options =>
{
    options.Cookie.SecurePolicy = googleCookieSecurePolicy;
    options.Cookie.SameSite = googleCookieSameSite;
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    // lưu thông tin user vào cookie sau khi login thành công
    options.SignInScheme = "External";
    options.ClientId = GetRequiredConfiguration(builder.Configuration, "Authentication:Google:ClientId");
    options.ClientSecret = GetRequiredConfiguration(builder.Configuration, "Authentication:Google:ClientSecret");
    options.CallbackPath = "/api/Authentication/oauth/google/callback";
    options.CorrelationCookie.SecurePolicy = googleCookieSecurePolicy;
    options.CorrelationCookie.SameSite = googleCookieSameSite;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(GetRequiredConfiguration(builder.Configuration, "Jwt:Key")))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static string GetRequiredConfiguration(IConfiguration configuration, string key)
{
    var value = configuration[key];
    if (string.IsNullOrWhiteSpace(value))
    {
        throw new InvalidOperationException($"{key} is missing. Add it to .env or environment variables.");
    }

    return value;
}

static void LoadEnvFile()
{
    var envPath = FindEnvFile();
    if (envPath is null)
    {
        return;
    }

    foreach (var rawLine in File.ReadAllLines(envPath))
    {
        var line = rawLine.Trim();
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
        {
            continue;
        }

        var equalsIndex = line.IndexOf('=');
        if (equalsIndex <= 0)
        {
            continue;
        }

        var key = line[..equalsIndex].Trim();
        var value = line[(equalsIndex + 1)..].Trim().Trim('"');

        Environment.SetEnvironmentVariable(key, value);
    }
}

static string? FindEnvFile()
{
    foreach (var startPath in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
    {
        var directory = new DirectoryInfo(startPath);
        while (directory is not null)
        {
            var envPath = Path.Combine(directory.FullName, ".env");
            if (File.Exists(envPath))
            {
                return envPath;
            }

            directory = directory.Parent;
        }
    }

    return null;
}
