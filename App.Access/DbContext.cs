
using App.Access.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.Access;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CountryName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Coordinates).IsRequired().HasMaxLength(50);
            entity.Property(x => x.CountryCode).IsRequired().HasMaxLength(10);
            entity.HasIndex(x => x.CountryCode).IsUnique();
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);

            entity.HasOne(x => x.Country)
                  .WithMany(x => x.Cities)
                  .HasForeignKey(x => x.CountryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "countries.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "countries.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}