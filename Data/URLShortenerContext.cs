using Microsoft.EntityFrameworkCore;
using url_shortener.Entities;

namespace url_shortener.Data;

public class UrlShortenerContext : DbContext
{
    public DbSet<XYZ> Urls { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Auth> Auth { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=urls.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                Urls = new List<XYZ>(),
                LimitUrl = 10
            });

        modelBuilder.Entity<Auth>().HasData(
            new Auth
            {
                Id = 1,
                Password = "admin",
                Role = "admin"
            }
        );
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Default",
            }
        );
    }
}