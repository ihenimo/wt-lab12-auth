using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab12_12auth_validation_savostikova.Data;
using wt_lab12_12auth_validation_savostikova.Models;

namespace wt_lab8_8razor_postgresql_savostikova.Pages.Products;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;
    public CreateModel(AppDbContext context) { _context = context; }

    [BindProperty]
    public Product Product { get; set; } = new();

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}