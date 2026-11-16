using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_savostikova.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. НАСТРОЙКА ПОДКЛЮЧЕНИЯ К БАЗЕ ДАННЫХ
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    
    // Проверяем порт: если Uri вернул -1 (как в Internal URL на Render), ставим стандартный 5432
    var dbPort = uri.Port > 0 ? uri.Port : 5432;

    connectionString =
        $"Host={uri.Host}; " +
        $"Port={dbPort}; " +
        $"Database={uri.AbsolutePath.TrimStart('/')}; " +
        $"Username={Uri.UnescapeDataString(userInfo[0])}; " +
        $"Password={Uri.UnescapeDataString(userInfo[1])}; " +
        "SSL Mode=Disable";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Подключение Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// ==========================================
// 2. АВТОМАТИЧЕСКОЕ ПРИМЕНЕНИЕ МИГРАЦИЙ
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ==========================================
// 3. НАСТРОЙКА КОНВЕЙЕРА ЗАПРОСОВ
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// ==========================================
// 4. НАСТРОЙКА ПОРТА ДЛЯ RENDER
// ==========================================
var appPort = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{appPort}");