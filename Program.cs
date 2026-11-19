using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Подключение к базе. На Render строка возьмется из переменных окружения!
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

var app = builder.Build();

// === БЛОК АВТОМАТИЧЕСКОГО ПРИМЕНЕНИЯ МИГРАЦИЙ (для Render) ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate(); // Создаст таблицы, если их нет
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка миграции: {ex.Message}");
    }
}
// =============================================================

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