using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace DBContext; 
using Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Flat> Flats => Set<Flat>();
    public DbSet<Price> Prices => Set<Price>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flat>()
            .HasMany(f => f.Prices)
            .WithOne(p => p.Flat)
            .HasForeignKey(p => p.FlatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}