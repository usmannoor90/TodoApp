using Microsoft.EntityFrameworkCore;
using TodoLibrary.EFModals;

namespace TodoLibrary.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add the missing DbSet property for Products
    public DbSet<Product> Products { get; set; } = null!;
}
