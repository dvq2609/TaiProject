var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Clean URLs (Xóa đuôi .html) Middleware
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    // 1. Chuyển hướng 301 từ .html sang clean URL (bỏ qua admin/api)
    if (!string.IsNullOrEmpty(path) && path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
    {
        if (!path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase) && !path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
        {
            var cleanPath = path.Substring(0, path.Length - 5);
            context.Response.Redirect(cleanPath + context.Request.QueryString, permanent: true);
            return;
        }
    }

    // 2. Viết lại URL ngầm: nếu URL sạch và có file tĩnh tương ứng trong wwwroot
    if (!string.IsNullOrEmpty(path) && 
        !path.Contains(".") && 
        !path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase) && 
        !path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
    {
        if (path.Length > 1 && path.EndsWith("/"))
        {
            path = path.TrimEnd('/');
        }

        var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var filePath = Path.Combine(env.WebRootPath, path.TrimStart('/') + ".html");
        
        if (File.Exists(filePath))
        {
            context.Request.Path = path + ".html";
        }
    }

    await next();
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
