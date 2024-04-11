using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Northwind.EntityModels;

public class NorthwindDb : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        #region To use SqlLite

        string database = "Northwind.db";
        string dir = Environment.CurrentDirectory;
        string path;

        if (dir.EndsWith("net8.0"))
        {
            // Running in the <project>\bin\<Debug|Release>\net8.0 directory.
            // D:\cs12dotnet8\my_code\Chapter11\LinqWithEFCore
            path = Path.Combine(database);
        }else
        {
            path = database;
        }

        path = Path.GetFullPath(path); // conver to absolute path
        if(!File.Exists(path))
        {
            throw new FileNotFoundException(message: $"{path} not found.", fileName: path);
        }

        #endregion

        optionsBuilder.UseSqlite($"Data Source={path}");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if(Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
        {
            modelBuilder.Entity<Product>()
                .Property(product => product.UnitPrice)
                .HasConversion<double>();
        }
    }



}