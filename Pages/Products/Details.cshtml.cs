using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Data;
using wt_lab12_12auth_validation_savostikova.Models;

namespace wt_lab8_8razor_postgresql_savostikova.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null) return NotFound();
        
        Product = product;
        return Page();
    }
}