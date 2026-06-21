using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

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
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseForwardedHeaders();

// Tự động chạy Migration & Seed dữ liệu mẫu khi khởi động trên Production
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BE.Models.ApplicationDbContext>();
        
        // 1. Tạo Database và chạy các bảng Migrations nếu chưa có
        dbContext.Database.Migrate();
        
        // 2. Chạy file seed_data.sql để nạp danh mục, sản phẩm mẫu nếu DB trống hoặc tất cả sản phẩm có Stock = 0 (lỗi seed trước đó)
        if (!dbContext.Products.Any() || dbContext.Products.All(p => p.Stock == 0))
        {
            var seedSqlPath = Path.Combine(AppContext.BaseDirectory, "seed_data.sql");
            if (!File.Exists(seedSqlPath))
            {
                seedSqlPath = Path.Combine(Directory.GetCurrentDirectory(), "seed_data.sql");
            }

            if (File.Exists(seedSqlPath))
            {
                var sqlContent = File.ReadAllText(seedSqlPath);
                var commands = sqlContent.Split(new[] { "GO\r\n", "GO\n", "go\r\n", "go\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cmd in commands)
                {
                    var cleanCmd = cmd.Trim();
                    if (!string.IsNullOrWhiteSpace(cleanCmd))
                    {
                        dbContext.Database.ExecuteSqlRaw(cleanCmd);
                    }
                }
                Console.WriteLine("Database seeded successfully from seed_data.sql!");
            }
            else
            {
                Console.WriteLine("seed_data.sql not found. Database seeding skipped.");
            }
        }

        // 3. Đảm bảo mật khẩu mặc định "123456" cho các tài khoản mẫu được mã hóa chính xác và kích hoạt tài khoản
        var adminUser = dbContext.Users.FirstOrDefault(u => u.Email == "admin@vuon.vn");
        if (adminUser != null)
        {
            adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            adminUser.Status = true;
        }

        var userA = dbContext.Users.FirstOrDefault(u => u.Email == "nguyenvana@email.com");
        if (userA != null)
        {
            userA.PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            userA.Status = true;
        }

        // 4. Kích hoạt toàn bộ các tài khoản chưa kích hoạt khác để tránh bị kẹt do lỗi gửi email trước đó
        var inactiveUsers = dbContext.Users.Where(u => !u.Status).ToList();
        foreach (var u in inactiveUsers)
        {
            u.Status = true;
            u.EmailConfirmedAt = DateTime.UtcNow;
            u.EmailConfirmationToken = null;
            u.EmailConfirmationTokenExpiresAt = null;
        }

        dbContext.SaveChanges();
        Console.WriteLine("Database integrity check completed. Pattern users reset/activated successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database migration/seeding: {ex.Message}");
    }
}

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
