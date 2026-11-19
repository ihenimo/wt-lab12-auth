using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Data;
using wt_lab12_12auth_validation_savostikova.Models;

namespace wt_lab8_8razor_postgresql_savostikova.Pages.Products;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Product> Products { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(p => p.Name.Contains(Search));
        }

        Products = await query.OrderBy(p => p.Name).ToListAsync();
    }
}