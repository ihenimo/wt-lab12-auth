// === БЛОК АВТОМАТИЧЕСКОГО СОЗДАНИЯ ТАБЛИЦ ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Пытаемся применить миграции
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
// ============================================