using Microsoft.EntityFrameworkCore;
using wt_lab12_12auth_validation_savostikova.Models;

namespace wt_lab12_12auth_validation_savostikova.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
}