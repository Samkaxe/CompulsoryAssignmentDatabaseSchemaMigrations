using EntityFrameworkCoreMigrations.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreMigrations.Context;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductRating> ProductRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=app.db");
}