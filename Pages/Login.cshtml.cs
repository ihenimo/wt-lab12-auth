using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Data; 

namespace wt_lab12_12auth_validation_savostikova.Pages;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Введите логин")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен содержать от 3 до 50 символов")]
    public string Login { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен содержать не менее 4 символов")]
    public string Password { get; set; } = "";

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // === АВТОМАТИЧЕСКОЕ СОЗДАНИЕ ПОЛЬЗОВАТЕЛЯ (если его нет) ===
        var userExists = await _context.Users.AnyAsync(u => u.Login == "student");
        if (!userExists)
        {
            Console.WriteLine("Пользователь student не найден. Создаю...");
            _context.Users.Add(new Models.User { Login = "student", Password = "1234" });
            await _context.SaveChangesAsync();
        }
        // =========================================================

        // Ищем пользователя
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == Login && x.Password == Password);

        if (user == null)
        {
            // Выводим в консоль, что именно мы искали и что есть в базе
            Console.WriteLine("--- ДИАГНОСТИКА ---");
            Console.WriteLine($"Ищем: Логин='{Login}', Пароль='{Password}'");
            var usersInDb = await _context.Users.ToListAsync();
            foreach (var u in usersInDb)
            {
                Console.WriteLine($"В базе есть: Логин='{u.Login}', Пароль='{u.Password}'");
            }
            
            ModelState.AddModelError("", "Неверный логин или пароль.");
            return Page();
        }

        HttpContext.Session.SetString("UserLogin", user.Login);
        return RedirectToPage("/Products/Index");
    }
}