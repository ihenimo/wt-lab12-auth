using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Data; // Убедитесь, что namespace ваш

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

// Создаем приложение (переменная app появляется ЗДЕСЬ)
var app = builder.Build();

// === БЛОК АВТОМАТИЧЕСКОГО СОЗДАНИЯ ТАБЛИЦ (для Render) ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate(); 
        Console.WriteLine("Миграции успешно применены.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка миграции: {ex.Message}");
        Console.WriteLine("Пытаемся создать базу с нуля (EnsureCreated)...");
        
        // Если миграции не сработали, принудительно создаем таблицы по моделям
        db.Database.EnsureCreated(); 
    }
}
// =========================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapRazorPages();

app.Run();