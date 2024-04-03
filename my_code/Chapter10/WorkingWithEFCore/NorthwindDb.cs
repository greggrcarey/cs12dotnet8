using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Northwind.EntityModels;

public class NorthwindDb : DbContext
{
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Supplier>? Suppliers { get; set; } 


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string databaseFile = "Northwind.db";
        string path = Path.Combine(Environment.CurrentDirectory, databaseFile);
        string connectionString = $"Data Source={path}";
        WriteLine($"Connection: {connectionString}");
        optionsBuilder.UseSqlite(connectionString);

        optionsBuilder.LogTo(WriteLine, new[] {RelationalEventId.CommandExecuting})

#if DEBUG
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
#endif
        ;

        optionsBuilder.UseLazyLoadingProxies();// Required for lazy loading ef queries
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(category => category.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        //Sqlite specific configuration
        if(Database.ProviderName?.Contains("Sqlite") ?? false)
        {
            modelBuilder.Entity<Product>()
                .Property(product => product.Cost)
                .HasConversion<double>();
        }

        //Global query filter to remove discontinued products
        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.Discontinued);


    }
}