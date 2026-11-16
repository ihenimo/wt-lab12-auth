using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_savostikova.Data;

var builder = WebApplication.CreateBuilder(args);

// Получение строки подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Подключение Razor Pages (страниц)
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Настройка конвейера запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Для картинок и CSS

app.UseRouting();

app.UseAuthorization();

// Включаем маршрутизацию для всех Razor Pages
app.MapRazorPages();

// Порт для Render
var appPort = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{appPort}");